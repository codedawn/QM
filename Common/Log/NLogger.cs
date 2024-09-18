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
        public static string ConfigPath = "Log/NLog.config";

        public NLogger(Type type)
        {
            if (_isFirst)
            {
                LogManager.Configuration = new XmlLoggingConfiguration(ConfigPath);
                _isFirst = false;
            }
            _logger = LogManager.GetLogger(type.FullName);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(Exception exception)
        {
            _logger.Error(exception);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }
    }
}
