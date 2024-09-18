using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using NLog.Config;

namespace QM
{
    public class NLogger : ILog
    {
        private readonly NLog.Logger _logger;
        private bool _isFirst = true;
        public static string configPath = "Log/NLog.config";
        public static LogLevel logLevel = LogLevel.Error;

        public NLogger(Type type)
        {
            if (_isFirst)
            {
                LogManager.Configuration = new XmlLoggingConfiguration(configPath);
                _isFirst = false;
            }
            _logger = LogManager.GetLogger(type.FullName);
        }

        public void Debug(string message)
        {
            if (logLevel <= LogLevel.Debug)
                _logger.Debug(message);
        }

        public void Error(string message)
        {
            if (logLevel <= LogLevel.Error)
                _logger.Error(message);
        }

        public void Error(Exception exception)
        {
            if (logLevel <= LogLevel.Error)
                _logger.Error(exception);
        }

        public void Info(string message)
        {
            if (logLevel <= LogLevel.Info)
                _logger.Info(message);
        }

        public void Warn(string message)
        {
            if (logLevel <= LogLevel.Warn)
                _logger.Warn(message);
        }
    }
}
