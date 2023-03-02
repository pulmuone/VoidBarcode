
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Plugin.Permissions;
using System.Collections.Generic;

namespace VoidBarcode.Droid
{
    [Activity(Label = "VoidBarcode", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        readonly int REQUEST_INSTALL_PERMISSION = 10;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            //알수 없는 소스 설치 여부
            if(PackageManager.CanRequestPackageInstalls())
            {
                SettingPermission();
            }
            else
            {
                var intent = new Intent(Android.Provider.Settings.ActionManageUnknownAppSources, 
                    Android.Net.Uri.Parse(string.Format("{0}{1}", "package:", Application.Context.PackageName)));
                //var intent = new Intent(Android.Provider.Settings.ActionManageUnknownAppSources);
                intent.SetData(Android.Net.Uri.Parse("package:" + Application.Context.PackageName));
                StartActivityForResult(intent, REQUEST_INSTALL_PERMISSION);                    
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if(requestCode == REQUEST_INSTALL_PERMISSION)
            {
                SettingPermission();
            }
        }

        //protected override void OnStart()
        private void SettingPermission()
        {
            //base.OnStart();                       

            //https://developer.android.com/guide/topics/security/permissions#normal-dangerous
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                List<string> permissions = new List<string>();

                if(Build.VERSION.SdkInt <= BuildVersionCodes.SV2)
                {
                    if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
                    {
                        permissions.Add(Manifest.Permission.WriteExternalStorage);
                    }
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}