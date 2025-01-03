using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
namespace SECSPublisher
{
    public class Logger
    {
        // 定义一个事件，当日志被写入时触发
        public static event EventHandler<LogEventArgs> LogWritten;
        public static readonly ILog SECSLogger;
        private static readonly string LogPath;
        static Logger()
        {
            LogPath = AppDomain.CurrentDomain.BaseDirectory + "Configuration/Log4net.config";
            try
            {
                FileInfo configFile = new FileInfo(LogPath);
                XmlConfigurator.ConfigureAndWatch(configFile);
                SECSLogger = LogManager.GetLogger("SECS");
            }
            catch (Exception)
            {
            }
        }
        public static void UiLogInfo(string log)
        {
            SECSLogger.Info(log);

            // 触发LogWritten事件
            LogWritten?.Invoke(null, new LogEventArgs(log));
        }
        public class LogEventArgs : EventArgs
        {
            public string LogMessage { get; private set; }

            public LogEventArgs(string logMessage)
            {
                LogMessage = logMessage;
            }
        }
    }
}
