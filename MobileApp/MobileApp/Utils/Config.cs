using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MobileApp.Utils
{
    public static class Config
    {
        public static void Initialize()
        {

            string json = File.ReadAllText("config.json");

            // Deserializuj zawartość pliku JSON do obiektu
            var config = JsonConvert.DeserializeObject<ConfigClass>(json);

            // Użyj wartości z obiektu config
            string apiKey = config.ApiKey;
            string apiUrl = config.ApiUrl;
            // itd.
        }

    }
    public class ConfigClass
    {
        public string ApiUrl { get; set; }
    }
}
