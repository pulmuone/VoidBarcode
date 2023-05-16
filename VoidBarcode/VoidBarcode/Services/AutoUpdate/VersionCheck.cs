using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using VoidBarcode.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VoidBarcode.Services.AutoUpdate
{
    public class VersionCheck
    {
        private static readonly VersionCheck instance = new VersionCheck();
        Version versionServer;
        Version versionClient;

        string url = string.Format(@"{0}{1}", GlobalSetting.Instance.MOBILEEndpoint.ToString(), @"/version.txt");

        private VersionCheck()
        {
            GetVersionClient();
        }

        public static VersionCheck Instance
        {
            get
            {
                return instance;
            }
        }

        //public async Task<bool> IsNetworkAccess()
        //{
        //    if (Connectivity.Profiles.Contains(ConnectionProfile.WiFi))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        await Application.Current.MainPage.DisplayAlert("WiFi 연결 오류", "WiFi 연결 후\n다시 처리해 주세요.", "OK");

        //        // Active Wi-Fi connection.
        //        return false;
        //    }
        //}

        public async Task<bool> IsUpdate()
        {
            await GetVersionServer();

            if (versionServer > versionClient)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateCheck()
        {
            if (versionServer == null)
            {
                await GetVersionServer();
            }

            if (versionServer > versionClient)
            {
                AutoUpdateView autoUpdatePage = new AutoUpdateView();
                await Application.Current.MainPage.Navigation.PushModalAsync(autoUpdatePage);
            }
        }

        /// <summary>
        /// Client 버전 확인
        /// </summary>
        /// <returns></returns>
        private Version GetVersionClient()
        {
            //var assembly = typeof(App).GetTypeInfo().Assembly;
            //var assemblyName = new AssemblyName(assembly.FullName);
            //versionClient = assemblyName.Version;
            versionClient = new Version(VersionTracking.CurrentVersion);

            return versionClient;
        }

        /// <summary>
        /// Server 버전 확인
        /// </summary>
        /// <returns></returns>
        private async Task<Version> GetVersionServer()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(new Uri(url));

                    if (response.IsSuccessStatusCode)
                    {
                        versionServer = new Version(response.Content.ReadAsStringAsync().Result.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                var properties = new Dictionary<string, string>
                {
                    {"BaseHttpService", "SendRequestAsync" },
                    {"UserID", "admin"}
                };
                Crashes.TrackError(ex, properties);
            }

            return versionServer;
        }
    }
}
