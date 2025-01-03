using SecsMessageEntity;
using SECSPublisher;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using SECSEntity;
using System.Threading.Tasks;
using System.Net;
namespace HSMSHandler
{
    /// <summary>
    /// 通过Socke建立与Secs设备的通讯
    /// </summary>
    public class HsmsConnection
    {
        private static readonly HsmsConnection _instance = new HsmsConnection();
        private static TcpClient _client=null;
        private static NetworkStream _stream=null;
        private static TcpListener tcp = null;
        DateTime HEARTBEATS = DateTime.Now;
        bool IsAStart = false;
        bool IsPStart = false;
        int timeout = 10;
        public bool IsConnection { get; private set; }
        private HsmsConnection()
        {
        }
        public void ReadTCPDataChannel()
        {
            try
            {
                if (_stream.CanRead && _stream.DataAvailable)
                {
                    byte[] length = new byte[4];
                    _stream.Read(length, 0, 4);
                    UInt32 size = BitConverter.ToUInt32(length.Reverse().ToArray(), 0);
                    byte[] receivedData = new byte[(int)size];
                    int bytesRead = _stream.Read(receivedData, 0, (int)size);
                    Logger.UiLogInfo($"RecvMessage:{string.Join(" ", length) +" "+ string.Join(" ", receivedData)}");
                    Logger.SECSLogger.Info($"RecvMessage:{string.Join(" ", length) + " " + string.Join(" ", receivedData)}");

                    SecsMessage recvMessage = new SecsMessage();
                    SecsMessage sendMessage = new SecsMessage();
                    recvMessage.ByteToSecsMessage(size, receivedData);
                    sendMessage.SecsHead = recvMessage.SecsHead;
                    switch (recvMessage.SecsHead.SType)
                    {
                        case SType.DateMessage:
                            break;
                        case SType.Select_req:
                            {
                                sendMessage.SecsHead.SType = SType.Select_rsp;
                                SendMessage(sendMessage.ToBytes());
                                return;
                            }
                        case SType.Select_rsp:
                            return;
                        case SType.Deselect_req:
                            break;
                        case SType.Deselect_rsp:
                            break;
                        case SType.Linktest_req:
                            {
                                sendMessage.SecsHead.SType = SType.Linktest_rsp;
                                SendMessage(sendMessage.ToBytes());
                                return;
                            }
                        case SType.Linktest_rsp:
                            break;
                        case SType.Reject_req:
                            break;
                        case SType.Separate_req:
                            {
                                IsConnection = false;
                                return;
                            }
                        default:
                            break;
                    }
                    if (recvMessage.SecsHead.IsReply)
                    {
                        sendMessage.SecsHead.Function+=1;
                        sendMessage.SecsHead.IsReply = false;
                        SendMessage(sendMessage.ToBytes());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SECSLogger.Error(DateTime.Now + " Connection ERORR:" + ex.Message);
                throw ex;
            }
        }
        public void SendMessage(Byte[] data)
        {
            try
            {
                if (IsConnection)
                {
                    _stream.Write(data, 0, data.Length);
                    _stream.Flush();
                    Logger.UiLogInfo($"SendMessage:{string.Join(" ", data)}");
                    Logger.SECSLogger.Info($"SendMessage:{string.Join(" ", data)}");
                }
            }
            catch (Exception ex)
            {
                Logger.SECSLogger.Error($"Function {ex.TargetSite.Name} Error:{ex.Message}");
            }

        }
        public void Disconnect()
        {
            try
            {
                IsAStart = false;
                IsPStart = false;
                if (_client!=null&&_stream!=null)
                {
                    _client.Close();
                    _stream.Close();
                    _stream = null;
                    _client = null;
                }
                if (tcp!=null)
                {
                    tcp.Stop();
                    tcp = null;
                }
                IsConnection = false;
                Logger.UiLogInfo($"Disconnect");
            }
            catch (Exception ex)
            {
                Logger.SECSLogger.Error($"Function {ex.TargetSite.Name} Error:{ex.Message}");
            }
        }
        public static HsmsConnection Instance
        {
            get { return _instance; }
        }
        public void OpenConnect()
        {
            if (IsAStart || IsPStart)
            {
                Disconnect();
            }
            switch (SETEntity.SecsObject.connectionModel)
            {
                case ConnectionModel.Active:
                    Task.Factory.StartNew(() =>
                    {
                        IsConnection = false;
                        IsAStart = true;
                        while (IsAStart)
                        {
                            try
                            {
                                if (!IsConnection)
                                {
                                    try
                                    {
                                        _client = new TcpClient(SETEntity.SecsObject.IP, SETEntity.SecsObject.Port);
                                        _stream = _client.GetStream();
                                        _stream.ReadTimeout = 15000;
                                        SecsMessage sm = new SecsMessage();
                                        sm.SecsHead.DeviceID = (UInt16)SETEntity.SecsObject.DeviceID;
                                        sm.SecsHead.SType = SType.Select_req;
                                        IsConnection = true;
                                        sm.SecsHead.IsReply = true;
                                        SendMessage(sm.ToBytes());
                                        Logger.UiLogInfo($"Connect Successful");
                                    }
                                    catch (SocketException e)
                                    {
                                        Logger.UiLogInfo($"{e}");
                                        Logger.UiLogInfo($"Retrying in 5 seconds.......");
                                        Logger.SECSLogger.Error($"TCP Error:{e}");
                                        Thread.Sleep(5000);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.SECSLogger.Error(DateTime.Now + " Connection ERORR:" + ex.Message);
                                    }
                                }
                                else
                                {
                                    ReadTCPDataChannel();
                                }

                            }
                            catch (SocketException e)
                            {
                                Logger.UiLogInfo($"{e}");
                                Logger.UiLogInfo($"Retrying in 5 seconds.......");
                                Logger.SECSLogger.Error($"TCP Error:{e}");
                                Thread.Sleep(5000);
                            }
                        }
                    });
                    break;
                case ConnectionModel.Passive:
                    Task.Factory.StartNew(() => {
                         tcp = new TcpListener(IPAddress.Parse(SETEntity.SecsObject.IP), SETEntity.SecsObject.Port);
                        tcp.Start();
                        Logger.UiLogInfo($"服务端已启动，正在监听端口:IP{SETEntity.SecsObject.IP}:{SETEntity.SecsObject.Port}");
                        IsConnection = false;
                        IsPStart = true;
                        while (IsPStart && tcp != null)
                        {
                            try
                            {
                                if (!IsConnection)
                                {
                                    try
                                    {
                                        _client = tcp.AcceptTcpClient();
                                        _stream = _client.GetStream();
                                        _stream.ReadTimeout = 15000;
                                        SecsMessage sm = new SecsMessage();
                                        sm.SecsHead.DeviceID = (UInt16)SETEntity.SecsObject.DeviceID;
                                        sm.SecsHead.SType = SType.Select_req;
                                        IsConnection = true;
                                        sm.SecsHead.IsReply = true;
                                        SendMessage(sm.ToBytes());
                                        Logger.UiLogInfo($"Connect Successful");
                                    }
                                    catch (SocketException e)
                                    {
                                        Logger.UiLogInfo($"{e}");
                                        Logger.UiLogInfo($"Retrying in 5 seconds.......");
                                        Logger.SECSLogger.Error($"TCP Error:{e}");
                                        Thread.Sleep(5000);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.SECSLogger.Error(DateTime.Now + " Connection ERORR:" + ex.Message);
                                    }
                                }
                                else
                                {
                                    ReadTCPDataChannel();
                                }

                            }
                            catch (SocketException e)
                            {
                                Logger.UiLogInfo($"{e}");
                                Logger.UiLogInfo($"Retrying in 5 seconds.......");
                                Logger.SECSLogger.Error($"TCP Error:{e}");
                                Thread.Sleep(5000);
                            }
                        }

                    });
                    break;
                default:
                    break;
            }
        }

    }


}
