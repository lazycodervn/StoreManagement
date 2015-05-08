﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using StoreManagement.Helpers.GeneralHelper;

namespace StoreManagement.Helpers
{
    public class ProjectAppSettings
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static string Domain
        {
            get
            {
                return GetWebConfigString("domain");
            }
        }
        public static string SiteName
        {
            get
            {
                return GetWebConfigString("siteName", "World Energy Reports");
            }
        }

        public static int RecordPerPage
        {
            get { return 20; }
        }
        public static int MaxItemsCountInFilter
        {
            get { return 30; }
        }
        public static string GetWebConfigString(string configName, string defaultValue = "")
        {
            var appValue = ConfigurationManager.AppSettings[configName];
            if (String.IsNullOrEmpty(appValue))
            {
                Logger.Trace(String.Format("Config Name {0} is using default value {1}    <add key=\"{0}\" value=\"{1}\" />", configName, defaultValue));
                return defaultValue;
            }
            else
            {
                return appValue;
            }
        }

        public static bool GetWebConfigBool(string configName, bool defaultValue = false)
        {
            //return !String.IsNullOrEmpty(WebConfigurationManager.AppSettings[configName]) ? WebConfigurationManager.AppSettings[configName].ToBool() : defaultValue;

            var configValue = defaultValue;
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings[configName]))
            {
                configValue = ConfigurationManager.AppSettings[configName].ToBool();
            }
            else
            {
                Logger.Trace(String.Format("Config Name {0} is using default value {1}   <add key=\"{0}\" value=\"{1}\" /> ", configName, defaultValue));
            }
            return configValue;

        }

        public static int GetWebConfigInt(string configName, int defaultValue = 0)
        {
            int configValue = -1;
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings[configName]))
            {
                configValue = ConfigurationManager.AppSettings[configName].ToInt();
            }
            else
            {
                Logger.Trace(String.Format("Config Name {0} is using default value {1}   <add key=\"{0}\" value=\"{1}\" /> ", configName, defaultValue));
            }
            return configValue == -1 ? defaultValue : configValue;
        }


        public static int CasheTinySeconds
        {
            get
            {
                return GetWebConfigInt("CasheTinySeconds", 1);
            }
        }


        public static int CasheShortSeconds
        {
            get
            {
                //return GetWebConfigInt("CasheShortSeconds", 1);
                return GetWebConfigInt("CasheShortSeconds", 10);
            }
        }

        public static int CasheMediumSeconds
        {
            get
            {
                return GetWebConfigInt("CasheMediumSeconds", 300);
            }
        }

        public static int CasheLongSeconds
        {
            get
            {
                return GetWebConfigInt("CasheLongSeconds", 1800);
            }
        }


        public static string ContentBase
        {
            get
            {

                return GetWebConfigString("ContentBase", "Sites.WER.");

            }
        }
    }
}