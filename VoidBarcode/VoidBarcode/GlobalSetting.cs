using System;
using System.Collections.Generic;
using System.Text;

namespace VoidBarcode
{
    public class GlobalSetting
    {
        private string DefaultEndpoint = "http://172.28.200.46:8080";
        //private string DefaultEndpoint = "http://172.28.200.104:8080";

        private string _baseEndpoint = string.Empty;
        private static readonly GlobalSetting _instance = new GlobalSetting();

        public GlobalSetting()
        {
#if DEBUG
            //DefaultEndpoint = "https://pulmuone.github.io/firebase";
            DefaultEndpoint = "http://172.28.200.46:8080";
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
            MOBILEEndpoint = $"{baseEndpoint}/PostgresqlWebApp"; //내부 테스트
            //MOBILEEndpoint = $"{baseEndpoint}";  //외부 테스트
            //MobileGetEndpoint = $"{baseEndpoint}//MobileGet";
            //MobileSetEndpoint = $"{baseEndpoint}//MobileSet";
        }
    }
}
