using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Common.Logging
{
    public class SerilogAdapter : IGesLogger
    {
        private readonly ILogger _logger;

        public SerilogAdapter(ILogger logger)
        {
            this._logger = logger;
        }

        public void Debug(string messageTemplate)
        {
            this._logger.Debug(messageTemplate);
        }

        public void Debug<T>(string messageTemplate, T propertyValue)
        {
            this._logger.Debug<T>(messageTemplate, propertyValue);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            this._logger.Debug(messageTemplate, propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate)
        {
            this._logger.Debug(exception, messageTemplate);
        }

        public void Debug<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            this._logger.Debug<T>(exception, messageTemplate, propertyValue);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            this._logger.Debug(exception, messageTemplate, propertyValues);
        }

        public void Error(string messageTemplate)
        {
            this._logger.Error(messageTemplate);
        }

        public void Error<T>(string messageTemplate, T propertyValue)
        {
            this._logger.Error<T>(messageTemplate, propertyValue);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            this._logger.Error(messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate)
        {
            this._logger.Error(exception, messageTemplate);
        }

        public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            this._logger.Error<T>(exception, messageTemplate, propertyValue);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            this._logger.Error(exception, messageTemplate, propertyValues);
        }

        public void Fatal(string messageTemplate)
        {
            this._logger.Fatal(messageTemplate);
        }

        public void Fatal<T>(string messageTemplate, T propertyValue)
        {
            this._logger.Fatal<T>(messageTemplate, propertyValue);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            this._logger.Fatal(messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate)
        {
            this._logger.Fatal(exception, messageTemplate);
        }

        public void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            this._logger.Fatal<T>(exception, messageTemplate, propertyValue);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            this._logger.Fatal(exception, messageTemplate, propertyValues);
        }

        public void Information(string messageTemplate)
        {
            this._logger.Information(messageTemplate);
        }

        public void Information<T>(string messageTemplate, T propertyValue)
        {
            this._logger.Information<T>(messageTemplate, propertyValue);
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            this._logger.Information(messageTemplate, propertyValues);
        }

        public void Information(Exception exception, string messageTemplate)
        {
            this._logger.Information(exception, messageTemplate);
        }

        public void Information<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            this._logger.Information<T>(exception, messageTemplate, propertyValue);
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            this._logger.Information(exception, messageTemplate, propertyValues);
        }

        public void Verbose(string messageTemplate)
        {
            this._logger.Verbose(messageTemplate);
        }

        public void Verbose<T>(string messageTemplate, T propertyValue)
        {
            this._logger.Verbose<T>(messageTemplate, propertyValue);
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            this._logger.Verbose(messageTemplate, propertyValues);
        }

        public void Verbose(Exception exception, string messageTemplate)
        {
            this._logger.Verbose(exception, messageTemplate);
        }

        public void Verbose<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            this._logger.Verbose<T>(exception, messageTemplate, propertyValue);
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            this._logger.Verbose(exception, messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate)
        {
            this._logger.Warning(messageTemplate);
        }

        public void Warning<T>(string messageTemplate, T propertyValue)
        {
            this._logger.Warning<T>(messageTemplate, propertyValue);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            this._logger.Warning(messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, string messageTemplate)
        {
            this._logger.Warning(exception, messageTemplate);
        }

        public void Warning<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            this._logger.Warning<T>(exception, messageTemplate, propertyValue);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            this._logger.Warning(exception, messageTemplate, propertyValues);
        }
    }
}
