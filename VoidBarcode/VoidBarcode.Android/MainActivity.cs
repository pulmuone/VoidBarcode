using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android;
using Android.Support.Design.Widget;
using Plugin.Permissions;

namespace VoidBarcode.Droid
{
    [Activity(Label = "VoidBarcode", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        View layout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());


            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                // Camera permission has not been granted
                //RequestWriteExternalStorage();
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage }, 0);
            }

            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != (int)Permission.Granted)
            {
                // Camera permission has not been granted
                //RequestCameraPermission();
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, 0);
            }
        }

        void RequestWriteExternalStorage()
        {
            //Log.Info(TAG, "CAMERA permission has NOT been granted. Requesting permission.");

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.WriteExternalStorage))
            {
                Snackbar.Make(layout, "Camera permission is needed to show the camera preview.", Snackbar.LengthIndefinite)
                        .SetAction("OK", new Action<View>(delegate (View obj)
                        {
                            ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage }, 0);
                        })).Show();
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage }, 0);
            }
        }

        void RequestCameraPermission()
        {
            //Log.Info(TAG, "CAMERA permission has NOT been granted. Requesting permission.");

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.Camera))
            {
                Snackbar.Make(layout, "Camera permission is needed to show the camera preview.", Snackbar.LengthIndefinite)
                        .SetAction("OK", new Action<View>(delegate (View obj)
                        {
                            ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, 0);
                        })).Show();
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, 0);
            }
        }
    }
}