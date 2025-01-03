using SECSPublisher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SecsMessageEntity
{
    public class SecsMessage
    {
        public SecsMessage()
        {
            SecsHead = new SecsHead();
            SecsRoot = new SecsRoot();
        }
        public SecsHead SecsHead { get; set; }
        public SecsRoot SecsRoot { get; set; }
        public byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                byte[] bytes;
                // 写入消息头
                bytes = BitConverter.GetBytes(SecsHead.DeviceID);
                Array.Reverse(bytes);
                bw.Write(bytes);
                if (SecsHead.IsReply)
                {
                    bw.Write((byte)(SecsHead.Stream + 128));
                }
                else
                {
                    bw.Write(SecsHead.Stream);
                }
                bw.Write(SecsHead.Function);
                bw.Write(SecsHead.Encode);
                bw.Write((byte)SecsHead.SType);
                bw.Write(SecsHead.Guid);

                // 写入消息体
                byte[] bodyBytes = SecsRoot.ToBytes();
                bw.Write(bodyBytes);

                byte[] exitbyte = ms.ToArray();
                ms.Position = 0;
                SecsHead.ByteLength = (UInt32)ms.Length;
                bytes = BitConverter.GetBytes(SecsHead.ByteLength);
                Array.Reverse(bytes);

                bw.Write(bytes);
                bw.Write(exitbyte);
                return ms.ToArray();
            }
        }
        public void ByteToSecsMessage(UInt32 length,byte[] data)
        {
            try
            {
                using (BinaryReader br = new BinaryReader(new MemoryStream(data), Encoding.Default))
                {
                    #region Secs Message Head Part Analysis
                    SecsHead.ByteLength = length;
                    SecsHead.DeviceID = BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0);
                    byte Stream = br.ReadByte();
                    if (Stream > 128)
                    {
                        SecsHead.IsReply = true;
                        SecsHead.Stream = (byte)(Stream - 128);
                    }
                    else
                    {
                        SecsHead.IsReply = false;
                        SecsHead.Stream = Stream;
                    }
                    SecsHead.Function = br.ReadByte();
                    SecsHead.Encode = br.ReadByte();
                    SecsHead.SType = (SType)br.ReadByte();
                    SecsHead.Guid = br.ReadBytes(4);
                    #endregion
                    if (br.BaseStream.Position >= br.BaseStream.Length)
                    {
                        return;
                    }
                    SecsRoot.BinaryParse(br);
                }
            }
            catch (Exception ex)
            {
                Logger.SECSLogger.Error($"Function {ex.TargetSite.Name} Error:{ex.Message}");
            }
        }
        public void StringToSecsMessage(string data)
        {
            try
            {
                using (StringReader sr = new StringReader(data))
                {
                    // 逐行读取字符串内容

                    string line = sr.ReadLine();
                    string s = line.Substring(line.IndexOf('S') + 1, line.IndexOf('F') - line.IndexOf('S') - 1);
                    string f = line.Substring(line.IndexOf('F') + 1, 2).Trim();
                    SecsHead.Stream = Convert.ToByte(s);
                    SecsHead.Function = Convert.ToByte(f);
                    if (Convert.ToByte(f) % 2 != 0)
                    {
                        SecsHead.IsReply = true;
                    }
                    SecsRoot.StringParse(sr);
                }
            }
            catch (Exception ex)
            {
                Logger.SECSLogger.Error($"Function {ex.TargetSite.Name} Error:{ex.Message}");
            }
        }
    }
    public class SecsHead
    {
        public uint ByteLength { get; set; }
        public UInt16 DeviceID { get; set; }
        public bool IsReply { get; set; }
        public Byte Stream { get; set; }
        public Byte Function { get; set; }
        public Byte Encode { get; set; }
        public SType SType { get; set; }
        public byte[] Guid { get; set; } = new byte[4];
    }
    public abstract class SecsBody
    {
        public abstract void BinaryParse(BinaryReader br);
        public abstract void StringParse(StringReader data);
        public abstract byte[] ToBytes();
    }
    public class SecsFunc
    {
        /// <summary>
        /// 将字节转为类型和长度
        /// </summary>
        /// <param name="b"></param>
        /// <returns>返回类型+长度</returns>
        public static int[] ConvertType(byte b)
        {
            int Length = 0;
            string binaryString = Convert.ToString(b, 2).PadLeft(8, '0');
            string octalPart = binaryString.Substring(0, 6);
            Length = Convert.ToInt32(binaryString.Substring(6, 2), 2);
            int i = Convert.ToInt32(octalPart, 2);
            string octalString = Convert.ToString(i, 8);
            return new int[] { Convert.ToInt32(octalString), Length };
        }
        public static byte ToByteType(int[] data)
        {
            int type = data[0];
            int length = data[1];

            // 将类型值八进制字符串转换为二进制字符串，并补齐6位
            string typeBinaryString = Convert.ToString(Convert.ToInt32(type.ToString(), 8), 2).PadLeft(6, '0');

            // 将长度值转换为二进制字符串，并补齐2位
            string lengthBinaryString = Convert.ToString(length, 2).PadLeft(2, '0');

            // 将类型值和长度值合并为一个8位二进制字符串
            string combinedBinaryString = typeBinaryString + lengthBinaryString;

            // 将二进制字符串转换为字节
            byte result = Convert.ToByte(combinedBinaryString, 2);

            return result;

        }
        public static DataType StringToDataType(string data)
        {
            DataType type = DataType.List;
            switch (data)
            {
                case "L":
                    {
                        type = DataType.List;
                        break;
                    }
                case "B":
                    {
                        type = DataType.Binary;
                        break;
                    }
                case "A":
                    {
                        type = DataType.ASCLL;
                        break;
                    }
                case "BOOLEAN":
                    {
                        type = DataType.Boolean;
                        break;
                    }
                case "J8":
                    {
                        type = DataType.JIS_8;
                        break;
                    }
                case "C2":
                    {
                        type = DataType.Any;
                        break;
                    }
                case "I8":
                    {
                        type = DataType.I8;
                        break;
                    }
                case "I1":
                    {
                        type = DataType.List;
                        break;
                    }
                case "I2":
                    {
                        type = DataType.I2;
                        break;
                    }
                case "I4":
                    {
                        type = DataType.I4;
                        break;
                    }
                case "U8":
                    {
                        type = DataType.U8;
                        break;
                    }
                case "U1":
                    {
                        type = DataType.U1;
                        break;
                    }
                case "U2":
                    {
                        type = DataType.U2;
                        break;
                    }
                case "U4":
                    {
                        type = DataType.U4;
                        break;
                    }
                case "F4":
                    {
                        type = DataType.F4;
                        break;
                    }
                case "F8":
                    {
                        type = DataType.F8;
                        break;
                    }
                default:
                    break;
            }
            return type;
        }

        public static byte[] DataTypeByte(DataType type, object value)
        {
            byte[] b = new byte[1];
            switch (type)
            {
                case DataType.List:
                    break;
                case DataType.Binary:
                    b[0] = 1;
                    break;
                case DataType.Boolean:
                    b[0] = 1;
                    break;
                case DataType.ASCLL:
                    {
                        string val = value as string;
                        if (val != null)
                        {
                            b[0] = (byte)val.Length;
                        }
                        break;
                    }
                case DataType.JIS_8:
                    {
                        string val = value as string;
                        if (val != null)
                        {
                            b[0] = (byte)val.Length;
                        }
                        break;
                    }
                case DataType.Any:
                    b[0] = 2;
                    break;
                case DataType.I8:
                    b[0] = 8;
                    break;
                case DataType.I1:
                    b[0] = 1;
                    break;
                case DataType.I2:
                    b[0] = 2;
                    break;
                case DataType.I4:
                    b[0] = 4;
                    break;
                case DataType.F8:
                    b[0] = 8;
                    break;
                case DataType.F4:
                    b[0] = 4;
                    break;
                case DataType.U8:
                    b[0] = 8;
                    break;
                case DataType.U1:
                    b[0] = 1;
                    break;
                case DataType.U2:
                    b[0] = 2;
                    break;
                case DataType.U4:
                    b[0] = 4;
                    break;
                default:
                    break;
            }
            return b;
        }
        public static object DataTypeValue(DataType type, object value)
        {
            object b = null;
            switch (type)
            {
                case DataType.List:
                    break;
                case DataType.Binary:
                    b = Convert.ToByte(value);
                    break;
                case DataType.Boolean:
                    b = Convert.ToBoolean(value);
                    break;
                case DataType.ASCLL:
                    b = value;
                    break;
                case DataType.JIS_8:
                    b = value;
                    break;
                case DataType.Any:
                    b = value;
                    break;
                case DataType.I8:
                    b = Convert.ToInt64(value);
                    break;
                case DataType.I1:
                    b = Convert.ToSByte(value);
                    break;
                case DataType.I2:
                    b = Convert.ToInt16(value);
                    break;
                case DataType.I4:
                    b = Convert.ToInt32(value);
                    break;
                case DataType.F8:
                    b = Convert.ToDouble(value);
                    break;
                case DataType.F4:
                    b = Convert.ToSingle(value);
                    break;
                case DataType.U8:
                    b = Convert.ToUInt64(value);
                    break;
                case DataType.U1:
                    b = Convert.ToByte(value);
                    break;
                case DataType.U2:
                    b = Convert.ToUInt16(value);
                    break;
                case DataType.U4:
                    b = Convert.ToUInt32(value);
                    break;
                default:
                    break;
            }
            return b;
        }
    }
    public class SecsRoot : SecsBody
    {
        public dynamic item { get; set; }
        public override void BinaryParse(BinaryReader br)
        {
            byte input = br.ReadByte(); // 收到的字节序列
            int[] result = SecsFunc.ConvertType(input);
            switch ((DataType)result[0])
            {
                case DataType.List:
                    {
                        item = new SecsList(result);
                        item.Length = result[1];
                        item.BinaryParse(br);
                        break;
                    }
                default:
                    {
                        item = new SecsItem((DataType)result[0]);
                        item.Length = result[1];
                        item.BinaryParse(br);
                        break;
                    }
            }
        }

        public override void StringParse(StringReader data)
        {
            string line = data.ReadLine();
            string Strtype = line.Substring(line.IndexOf('<') + 1).Split(',')[0];
            string Count = line.Substring(line.IndexOf('<') + 1).Split(',')[1];
            DataType type = SecsFunc.StringToDataType(Strtype);
            switch (type)
            {
                case DataType.List:
                    {
                        item = new SecsList(new int[] { (int)type, 1 });
                        item.Length = Convert.ToInt32(Count);
                        item.StringParse(data);
                        break;
                    }
                default:
                    {
                        item = new SecsItem(type);
                        item.Length = Convert.ToInt32(Count);
                        break;
                    }
            }

        }

        public override byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                if (item != null)
                {
                    byte[] itemBytes = item.ToBytes();
                    bw.Write(itemBytes);
                }

                return ms.ToArray();
            }
        }
    }
    public class SecsList : SecsBody
    {
        public int Length { get; set; }
        public SecsList(int[] result)
        {
            Items = new List<dynamic>();
            this.result = result;
        }
        public List<dynamic> Items { get; private set; }
        public int[] result { get; private set; }
        public override void BinaryParse(BinaryReader br)
        {
            byte[] input = br.ReadBytes(Length);
            if (input[0] == 0 || br.BaseStream.Position >= br.BaseStream.Length)
            {
                return;
            }
            for (int i = 0; i < input[0]; i++)
            {
                result = SecsFunc.ConvertType(br.ReadByte());
                dynamic item = null;
                switch ((DataType)result[0])
                {
                    case DataType.List:
                        {
                            item = new SecsList(result);
                            item.Length = result[1];
                            item.BinaryParse(br);
                            break;
                        }
                    default:
                        {
                            item = new SecsItem((DataType)result[0]);
                            item.Length = result[1];
                            item.BinaryParse(br);
                            break;
                        }
                }
                Items.Add(item);

            }

        }

        public override byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(SecsFunc.ToByteType(result));
                bw.Write((byte)Items.Count);
                foreach (var item in Items)
                {
                    byte[] itemBytes = item.ToBytes();
                    bw.Write(itemBytes);
                    byte[] data = ms.ToArray();
                }

                return ms.ToArray();
            }
        }

        public override void StringParse(StringReader data)
        {
            for (int i = 0; i < Length; i++)
            {
                dynamic item = null;
                string line = data.ReadLine();
                int startindex = line.IndexOf('<') + 1;
                if (startindex<=0)
                {
                    i = i - 1;
                    continue;
                }
                string Strtype = line.Substring(startindex).Split(',')[0];
                DataType type = SecsFunc.StringToDataType(Strtype);
                switch (type)
                {
                    case DataType.List:
                        {
                            item = new SecsList(new int[] { (int)type, 1 });
                            result = new int[] { (int)type, 1 };
                            int Position = line.IndexOf('>');
                            if (Position>0)
                            {
                                //string hh = line.Substring(startindex, Position- startindex);
                                //item.Length = Convert.ToInt32(hh.Split(',')[1]);
                                item.Length = 0;
                            }
                            else
                            {
                                item.Length = Convert.ToInt32(line.Substring(startindex).Split(',')[1]);

                            }
                            item.StringParse(data);
                            break;
                        }

                    default:
                        {
                            int index = line.IndexOf('\'') + 1;
                            int endindex = line.Length - 2;
                            object value = SecsFunc.DataTypeValue(type, line.Substring(index, endindex - index));
                            item = new SecsItem(type, value, SecsFunc.DataTypeByte(type, value));
                            item.Length = Convert.ToInt32(1);

                            break;
                        }
                }
                Items.Add(item);

            }
        }
    }
    public class SecsItem : SecsBody
    {
        public int Length { get; set; }
        public SecsItem(DataType dt)
        {
            type = dt;
        }
        public SecsItem(DataType dt, object value, byte[] count)
        {
            type = dt;
            this.value = value;
            this.count = count;
        }
        public DataType type { get; private set; }
        public dynamic value { get; private set; }
        public byte[] count { get; private set; }
        public override void BinaryParse(BinaryReader br)
        {
            count = br.ReadBytes(Length);
            if (count[0] == 0 || br.BaseStream.Position >= br.BaseStream.Length)
            {
                return;
            }
            byte[] DataValue = br.ReadBytes(count[0]);
            switch (type)
            {
                case DataType.List:
                    break;
                case DataType.Binary:
                    value = DataValue[0];
                    break;
                case DataType.Boolean:
                    value = BitConverter.ToBoolean(DataValue.Reverse().ToArray(), 0);
                    break;
                case DataType.ASCLL:
                    value = Encoding.ASCII.GetString(DataValue);
                    break;
                case DataType.JIS_8:
                    break;
                case DataType.Any:
                    value = BitConverter.ToChar(DataValue.Reverse().ToArray(), 0);
                    break;
                case DataType.I8:
                    value = BitConverter.ToInt64(DataValue.Reverse().ToArray(), 0);
                    break;
                case DataType.I1:
                    value = DataValue[0];
                    break;
                case DataType.I2:
                    value = BitConverter.ToInt16(DataValue.Reverse().ToArray(), 0);
                    break;
                case DataType.I4:
                    value = BitConverter.ToInt32(DataValue.Reverse().ToArray(), 0);
                    break;
                case DataType.F8:
                    value = BitConverter.ToDouble(DataValue.Reverse().ToArray(), 0);
                    break;
                case DataType.F4:
                    value = BitConverter.ToSingle(DataValue.Reverse().ToArray(), 0);
                    break;
                case DataType.U8:
                    value = BitConverter.ToUInt64(DataValue.Reverse().ToArray(), 0);
                    break;
                case DataType.U1:
                    value = DataValue[0];
                    break;
                case DataType.U2:
                    value = BitConverter.ToUInt16(DataValue.Reverse().ToArray(), 0);
                    break;
                case DataType.U4:
                    value = BitConverter.ToUInt32(DataValue.Reverse().ToArray(), 0);
                    break;
                default:
                    break;
            }
        }

        public override byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(SecsFunc.ToByteType(new int[] { (int)type, Length }));
                bw.Write(count[0]);
                byte[] valueBytes = GetValueBytes();
                bw.Write(valueBytes);

                return ms.ToArray();
            }
        }
        private byte[] GetValueBytes()
        {
            byte[] bytes;
            switch (type)
            {
                case DataType.Binary:
                    return new byte[] { (byte)value };
                case DataType.Boolean:
                    bytes = BitConverter.GetBytes((bool)value);
                    break;
                case DataType.ASCLL:
                    bytes = Encoding.ASCII.GetBytes((string)value);
                    break;
                case DataType.JIS_8:
                case DataType.Any:
                    bytes = Encoding.Unicode.GetBytes((string)value);
                    break;
                case DataType.I1:
                    bytes = new byte[] { (byte)value };
                    break;
                case DataType.I2:
                    bytes = BitConverter.GetBytes((short)value);
                    Array.Reverse(bytes);
                    break;
                case DataType.I4:
                    bytes = BitConverter.GetBytes((int)value);
                    Array.Reverse(bytes);
                    break;
                case DataType.I8:
                    bytes = BitConverter.GetBytes((long)value);
                    Array.Reverse(bytes);
                    break;
                case DataType.F4:
                    bytes = BitConverter.GetBytes((float)value);
                    Array.Reverse(bytes);
                    break;
                case DataType.F8:
                    bytes = BitConverter.GetBytes((double)value);
                    Array.Reverse(bytes);
                    break;
                case DataType.U1:
                    bytes = new byte[] { (byte)value };
                    break;
                case DataType.U2:
                    bytes = BitConverter.GetBytes((ushort)value);
                    Array.Reverse(bytes);
                    break;
                case DataType.U4:
                    bytes = BitConverter.GetBytes((uint)value);
                    Array.Reverse(bytes);
                    break;
                case DataType.U8:
                    bytes = BitConverter.GetBytes((ulong)value);
                    Array.Reverse(bytes);
                    break;
                default:
                    throw new NotImplementedException($"Conversion for data type {type} is not implemented.");
            }
            return bytes;
        }

        public override void StringParse(StringReader data)
        {
        }
    }
    public enum DataType
    {
        List = 0,
        Binary = 10,
        Boolean = 11,
        ASCLL = 20,
        JIS_8 = 21,
        Any = 22,

        I8 = 30,
        I1 = 31,
        I2 = 32,
        I4 = 34,

        F8 = 40,
        F4 = 44,

        U8 = 50,
        U1 = 51,
        U2 = 52,
        U4 = 54,
    }
    public enum SType
    {
        DateMessage = 0,
        Select_req = 1,
        Select_rsp = 2,
        Deselect_req = 3,
        Deselect_rsp = 4,
        Linktest_req = 5,
        Linktest_rsp = 6,
        Reject_req = 7,
        Separate_req = 9,
    }
}
