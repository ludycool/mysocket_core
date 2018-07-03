using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace consoletest
{
    public class ConfigurationManager
    {
        static string filename = "config.json";
        private static IConfigurationRoot configurationRoot;

        public ConfigurationManager() { }

        public ConfigurationManager(string _filename)
        {

            filename = _filename;
        }


        static IConfigurationRoot GetRoot()
        {
            if (configurationRoot == null)
            {
                var builder = new ConfigurationBuilder().AddJsonFile(filename)
               .AddInMemoryCollection(new[]
               {
                           KeyValuePair.Create("the-key", "the-value"),
               });
                configurationRoot = builder.Build();
            }
            return configurationRoot;
        }


        public static string GetJsonValue(string key)
        {

            return GetRoot()[key];
        }


        public static string GetJsonValue(string parentpath, string key)
        {
            return GetJsonValue(string.Format("{0}:{1}", parentpath, key));
        }

        public static IConfigurationSection GetSection(string key)
        {
            return GetRoot().GetSection(key);
        }
    }
}
