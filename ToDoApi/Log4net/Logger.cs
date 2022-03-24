using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace ToDoApi.Log4net
{
    public class Logger
    {
        private static readonly string LOG_CONFIG_FILE = @"Log4net\log4net.config";

        private static ILogger _log = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), typeof(Logger));
        //public static ILog GetLogger(Type type)
        //{
        //    return LogManager.GetLogger(type);
        //}

        public static void Info(object message)
        {
            SetLog4NetConfiguration();
            _log.Log(typeof(Logger), Level.Info, message, null);
        }

        public static void Error(object message)
        {
            SetLog4NetConfiguration();
            _log.Log(typeof(Logger), Level.Error, message, null);
        }
        public static void Debug(object message)
        {
            SetLog4NetConfiguration();
            _log.Log(typeof(Logger), Level.Debug, message, null);
        }

        private static void SetLog4NetConfiguration()
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(LOG_CONFIG_FILE));

            var repo = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }
    }
}
