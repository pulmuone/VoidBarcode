
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using System.Collections.Generic;

namespace VoidBarcode.Droid
{
    [Activity(Label = "VoidBarcode", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

        }

        protected override void OnStart()
        {
            base.OnStart();

            
            //'출처를 알 수 없는 앱'는 아래 코드로 알아 올 수 있습니다.nResult 가 1이면  '출처를 알 수 없는 앱'이 활성화 되어 있는 상태 입니다.
            //bool unknownSource = false;
            //if ((int)Build.VERSION.SdkInt < 17)
            //{
            //    unknownSource = Android.Provider.Settings.Secure.GetInt(ContentResolver, Android.Provider.Settings.Secure.InstallNonMarketApps, 0) == 1;
            //}
            //else
            //{
            //    unknownSource = Android.Provider.Settings.Global.GetInt(ContentResolver, Android.Provider.Settings.Global.InstallNonMarketApps, 0) == 1;
            //}

            //unknownSource = Android.Provider.Settings.Secure.GetInt(ContentResolver, Android.Provider.Settings.Secure.InstallNonMarketApps) == 1;

            //Intent intent = new Intent(Android.Provider.Settings.ActionManageUnknownAppSources);
            //StartActivity(intent);

            //https://developer.android.com/guide/topics/security/permissions#normal-dangerous
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                List<string> permissions = new List<string>();

                if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
                {
                    permissions.Add(Manifest.Permission.WriteExternalStorage);
                }

                if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != (int)Permission.Granted)
                {
                    permissions.Add(Manifest.Permission.Camera);
                }


                if (permissions.Count > 0)
                {
                    ActivityCompat.RequestPermissions(this, permissions.ToArray(), 1);
                }
            }
        }
    }
}