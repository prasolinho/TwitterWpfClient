using System;
using System.Configuration;

namespace TwitterWpfClient.Framework
{
    public class AppSettings : IAppSettings
    {
        private static readonly IAppSettings instance = new AppSettings();
        private static readonly object lockSettings = new object ();
        public static IAppSettings Instance
        {
            get
            {
                return instance;
            }
        }

        public string ConsumerKey
        {
            get
            {
                lock (lockSettings)
                {
                    return ConfigurationManager.AppSettings["consumerKey"];
                }
            }
        }

        public string ConsumerSecret
        {
            get
            {
                lock (lockSettings)
                {
                    return ConfigurationManager.AppSettings["consumerSecret"];
                }
            }
        }

        public string Token
        {
            get
            {
                lock (lockSettings)
                {
                    return ConfigurationManager.AppSettings["token"];
                }
            }
        }

        public string TokenSecret
        {
            get
            {
                lock (lockSettings)
                {
                    return ConfigurationManager.AppSettings["tokenSecret"];
                }
            }
        }

        /// <summary>
        /// Singleton, dlatego prywatny konstruktor
        /// </summary>
        private AppSettings()
        {
        }

        public bool AddSetting(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }
    }
}