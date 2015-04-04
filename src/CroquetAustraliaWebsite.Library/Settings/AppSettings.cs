using System;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using EmptyStringGuard;

namespace CroquetAustraliaWebsite.Library.Settings
{
    public class AppSettings
    {
        private readonly string _appSettingsPrefix;

        public AppSettings()
            : this("")
        {
        }

        public AppSettings([AllowEmpty] string appSettingsPrefix)
        {
            _appSettingsPrefix = appSettingsPrefix;

            if (_appSettingsPrefix != "")
            {
                _appSettingsPrefix += ":";
            }
        }

        public string Get(string key)
        {
            var fullKey = _appSettingsPrefix + key;
            var value = WebConfigurationManager.AppSettings[fullKey];

            if (value != null)
            {
                return value;
            }
            if (WebConfigurationManager.AppSettings.AllKeys.Any(c => c.StartsWith(_appSettingsPrefix)))
            {
                throw new Exception(string.Format("Value for AppSetting {0} cannot be null.", _appSettingsPrefix + key));
            }
            throw new Exception(string.Format("AppSettings[{0}] is empty. Maybe you need to create AppSettings.config. See AppSettings.Example.config.", _appSettingsPrefix));
        }

        public string GetDirectory(string key, HttpServerUtility server)
        {
            return server.MapPath(Get(key));
        }
    }
}