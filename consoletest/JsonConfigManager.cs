using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace consoletest
{
    public class JsonConfigManager
    {
        private string filename = "config.json";
        private static JsonConfigManager _instance;
        public static JsonConfigManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new JsonConfigManager();
                return _instance;
            }
        }

        private JObject _configures;
        public JObject Configures
        {
            get
            {
                if (_configures == null)
                    _configures = ReadConfig();


                return _configures;
            }
        }
        private string _conf_Str;
        public string conf_Str
        {
            get
            {
                if (string.IsNullOrEmpty(_conf_Str))
                {
                    using (FileStream fs = new FileStream(filename, FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            _conf_Str = sr.ReadToEnd();
                        }
                    }
                }

                return _conf_Str;
            }
        }
        /// <summary>
        ///  json格式转换 jobject
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static JObject FromJson(string strJson)
        {

            JObject jo = (JObject)JsonConvert.DeserializeObject(strJson);
            //string zone = jo["beijing"]["zone"].ToString();
            //string zone_en = jo["beijing"]["zone_en"].ToString(); 
            return jo;
        }
        private JObject ReadConfig()
        {
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        _conf_Str = sr.ReadToEnd();
                        return (JObject)JsonConvert.DeserializeObject(_conf_Str);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private JsonConfigManager()
        {
        }

    }
}
