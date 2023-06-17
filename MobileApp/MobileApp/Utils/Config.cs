using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MobileApp.Utils
{
    public static class Config
    {
        public static ConfigClass Data { get; set; }
        public static void Initialize()
        {

           // string json = File.ReadAllText("Utils/config.json");

            // Deserializuj zawartość pliku JSON do obiektu
            //Data = JsonConvert.DeserializeObject<ConfigClass>(json);

           
            // itd.
        }

    }
    public class ConfigClass
    {
        public string ApiUrl { get; set; }
    }
}
