using System;
using System.Collections.Generic;
using System.Text;

namespace VoidBarcode
{
    public class GlobalSetting
    {
        private string DefaultEndpoint = "https://pulmuone.github.io/firebase"; //full주소 쓰자
        //private string DefaultEndpoint = "http://192.168.0.106:8080/app"; //full주소 쓰자
        //private string DefaultEndpoint = "http://192.168.0.106:8080";

        private string _baseEndpoint = string.Empty;
        private static readonly GlobalSetting _instance = new GlobalSetting();

        public GlobalSetting()
        {
#if DEBUG
            //DefaultEndpoint = "https://pulmuone.github.io/firebase";
            DefaultEndpoint = "http://172.34.34.144:8080/xamarin";
#endif
            AuthToken = "INSERT AUTHENTICATION TOKEN";
            BaseEndpoint = DefaultEndpoint;
        }

        public static GlobalSetting Instance
        {
            get { return _instance; }
        }

        public string BaseEndpoint
        {
            get { return _baseEndpoint; }
            set
            {
                _baseEndpoint = value;
                UpdateEndpoint(_baseEndpoint);
            }
        }
        public string AuthToken { get; set; }

        public string ClientId { get { return "VoidBarcode"; } }

        public string MOBILEEndpoint { get; set; }

        //public string MobileGetEndpoint { get; set; }
        //public string MobileSetEndpoint { get; set; }

        private void UpdateEndpoint(string baseEndpoint)
        {
            MOBILEEndpoint = $"{baseEndpoint}";
        }
    }
}
