﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.Content;
using Android.Widget;
using Java.IO;
using System;
using System.Net;

namespace VoidBarcode.Droid
{
    [Activity(Label = "자동 업데이트", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AutoUpdateActivity : Activity
    {
        public static event Action OnUpdateCompleted;

        private ProgressBar progressBar;
        private ProgressBar progressBar2;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AutoUpdateLayout);

            // Create your application here            
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            progressBar2 = FindViewById<ProgressBar>(Resource.Id.progressBar2);

            FileDelete();

            CheckUpdate();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnUpdateCompleted = null;
        }

        private void FileDelete()
        {
            File file = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString());
            IFilenameFilter filter = new AutoUpdateFileFilter();
            File[] files = file.ListFiles(filter);
            if (files != null && files.Length > 0)
            {
                foreach (var item in files)
                {
                    if (item.IsFile)
                    {
                        if (item.Name.ToString().StartsWith("com.gwise.voidbarcode"))
                        {
                            using (File fileDel = new File(item.ToString()))
                            {
                                if (fileDel.Exists())
                                {
                                    fileDel.Delete();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CheckUpdate()
        {
            var ad = new AlertDialog.Builder(this).Create();
            try
            {
                var webClient = new WebClient();
                var url = new System.Uri(string.Format(@"{0}{1}", GlobalSetting.Instance.MOBILEEndpoint.ToString(), @"/com.gwise.voidbarcode.apk"));
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                webClient.DownloadFileAsync(url, Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads) + "/com.gwise.voidbarcode.apk");
                webClient.DownloadFileCompleted += (s, e) =>
                {
                    if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.N)
                    {
                        Intent intent = new Intent(Intent.ActionInstallPackage);
                        //intent.SetDataAndType(FileProvider.GetUriForFile(this.ApplicationContext, "com.gwise.voidbarcode.fileprovider", new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads) + "/com.gwise.voidbarcode.apk")), "application/vnd.android.package-archive");
                        intent.SetData(FileProvider.GetUriForFile(this.ApplicationContext, "com.gwise.voidbarcode.fileprovider", new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads) + "/com.gwise.voidbarcode.apk")));
                        intent.SetFlags(ActivityFlags.GrantReadUriPermission);
                        intent.SetFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                    }
                    else
                    {
                        Intent intent = new Intent(Intent.ActionView);
                        intent.SetDataAndType(Android.Net.Uri.FromFile(new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads) + "/com.gwise.voidbarcode.apk")), "application/vnd.android.package-archive");
                        intent.SetFlags(ActivityFlags.NewTask); // ActivityFlags.NewTask 이 옵션을 지정해 주어야 업데이트 완료 후에 [열기]라는 화면이 나온다.
                        StartActivity(intent);
                    }

                    //var apkUri = FileProvider.GetUriForFile(this, this.ApplicationContext.PackageName + ".provider", new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads) + "/com.gwise.voidbarcode.apk"));
                    //Intent intent = new Intent(Intent.ActionInstallPackage);
                    //intent.SetData(apkUri);
                    //intent.SetFlags(ActivityFlags.GrantReadUriPermission);
                    //intent.SetFlags(ActivityFlags.NewTask); // AddFlags(ActivityFlags.NewTask); // ActivityFlags.NewTask 이 옵션을 지정해 주어야 업데이트 완료 후에 [열기]라는 버튼이 보인다.
                    //StartActivity(intent);

                    OnUpdateCompleted?.Invoke();
                    this.Finish();
                };
            }
            catch (System.Exception ex)
            {
                ad = new AlertDialog.Builder(this).Create();
                ad.SetTitle("INFO");
                ad.SetMessage(ex.Message);
                ad.SetCanceledOnTouchOutside(true);
                ad.Show();
            }
            finally
            {
            }
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            System.Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
                 (string)e.UserState,
                 e.BytesReceived,
                 e.TotalBytesToReceive,
                 e.ProgressPercentage);

            int length = Convert.ToInt32(e.TotalBytesToReceive.ToString());
            int prog = Convert.ToInt32(e.BytesReceived.ToString());
            int perc = Convert.ToInt32(e.ProgressPercentage.ToString());

            try
            {
                progressBar.Max = 100;
                progressBar.Progress = perc;

                progressBar2.Max = 100;
                progressBar2.Progress = perc;
            }
            finally
            {

            }
        }
    }

    public class AutoUpdateFileFilter : Java.Lang.Object, IFilenameFilter
    {
        public bool Accept(File dir, string name)
        {
            if (name.StartsWith("com.gwise.voidbarcode") && name.EndsWith(".apk"))
            {
                return true;
            }
            return false;
        }
    }

}