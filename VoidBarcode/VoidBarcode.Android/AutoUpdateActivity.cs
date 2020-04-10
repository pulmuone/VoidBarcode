using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Support.V4.Content;
using Android.Widget;
using Java.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VoidBarcode.Droid
{
    [Activity(Label = "자동 업데이트", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AutoUpdateActivity : Activity
    {
        public static event Action OnUpdateCompleted;
        public static long downloadId = -1L;
        private DownloadReceiver receiver;

        private ProgressBar progressBar;
        //private ProgressBar progressBar2;
        private TextView textView1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AutoUpdateLayout);

            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            textView1 = FindViewById<TextView>(Resource.Id.textView1);

            progressBar.Max = 100;
            textView1.Text = "0 %";

            //FileDelete();

            //SaveQ();


            ContentValues values = new ContentValues();
            values.Put(MediaStore.MediaColumns.DisplayName, string.Format("{0}{1}", Application.Context.PackageName, ".apk"));
            values.Put(MediaStore.MediaColumns.MimeType, "application/vnd.android.package-archive");
            values.Put(MediaStore.MediaColumns.RelativePath, "Download");
            values.Put(MediaStore.MediaColumns.IsPending, 1);

            ContentResolver contentResolver = this.ContentResolver;
            Android.Net.Uri item = contentResolver.Insert(MediaStore.Downloads.GetContentUri(MediaStore.VolumeExternal), values);

            ParcelFileDescriptor pfd = contentResolver.OpenFileDescriptor(item, "w", null);


            var manager = DownloadManager.FromContext(this);
            //var manager = (DownloadManager)GetSystemService(Context.DownloadService);

            var url = new System.Uri(string.Format(@"{0}{1}", GlobalSetting.Instance.MOBILEEndpoint.ToString(), string.Format("{0}{1}{2}", "/", Application.Context.PackageName, ".apk")));

            var request = new DownloadManager.Request(Android.Net.Uri.Parse(url.ToString()));
            request.SetMimeType("application/vnd.android.package-archive");
            request.SetTitle("App Download");
            request.SetDescription("File Downloading...");
            //request.SetAllowedNetworkTypes(DownloadNetwork.Wifi | DownloadNetwork.Mobile);
            request.SetAllowedNetworkTypes(DownloadNetwork.Wifi);
            request.SetNotificationVisibility(DownloadVisibility.Visible);
            request.SetAllowedOverMetered(true);
            request.SetAllowedOverRoaming(true);
            //request.SetDestinationUri(Android.Net.Uri.FromFile(new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath + string.Format("{0}{1}{2}", "/", Application.Context.PackageName, ".apk"))));
            request.SetDestinationUri(item);
            //request.SetDestinationInExternalPublicDir(this.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).ToString(), string.Format("{0}{1}", Application.Context.PackageName, ".apk"));


            downloadId = manager.Enqueue(request);

            Task.Run(() =>
            {
                for (; ; )
                {
                    var query = new DownloadManager.Query();
                    query.SetFilterById(downloadId);
                    ICursor cursor = manager.InvokeQuery(query);
                    if (cursor.MoveToFirst())
                    {
                        var soFar = cursor.GetDouble(cursor.GetColumnIndex(DownloadManager.ColumnBytesDownloadedSoFar));
                        var total = cursor.GetDouble(cursor.GetColumnIndex(DownloadManager.ColumnTotalSizeBytes));
                        RunOnUiThread(() =>
                        {
                            progressBar.SetProgress(System.Convert.ToInt32(soFar / total * 100), true);
                            textView1.Text = string.Format("{0} %", Convert.ToInt32(soFar / total * 100));
                        });
                        System.Console.WriteLine(soFar.ToString());

                        if (soFar.Equals(total))
                        {
                            break;
                        }
                    }
                    Thread.Sleep(500);
                }

                values.Clear();
                values.Put(MediaStore.DownloadColumns.IsPending, 0);
                contentResolver.Update(item, values, null, null);
            });

        }



        private void SaveQ()
        {
            var url = new System.Uri(string.Format(@"{0}{1}", GlobalSetting.Instance.MOBILEEndpoint.ToString(), string.Format("{0}{1}{2}", "/", Application.Context.PackageName, ".apk")));

            ContentValues values = new ContentValues();
            values.Put(MediaStore.MediaColumns.DisplayName, string.Format("{0}{1}", Application.Context.PackageName, ".apk"));
            values.Put(MediaStore.MediaColumns.MimeType, "application/vnd.android.package-archive");
            values.Put(MediaStore.MediaColumns.RelativePath, "Download");
            values.Put(MediaStore.MediaColumns.IsPending, 1);

            ContentResolver contentResolver = this.ContentResolver;
            Android.Net.Uri item = contentResolver.Insert(MediaStore.Downloads.GetContentUri(MediaStore.VolumeExternal), values);

            ParcelFileDescriptor pfd = contentResolver.OpenFileDescriptor(item, "w", null);

            if (pfd == null)
            {

            }
            else
            {
                //ToDo
                Stream output = contentResolver.OpenOutputStream(item);

                //DataOutputStream(output);
                //Java.IO.File file = new Java.IO.File(output);



                values.Clear();
                values.Put(MediaStore.DownloadColumns.IsPending, 0);
                contentResolver.Update(item, values, null, null);
            }
        }


        protected override void OnResume()
        {
            base.OnResume();

            // Registers BroadcastReceiver to respond to completed downloads.
            var filter = new IntentFilter(DownloadManager.ActionDownloadComplete);
            receiver = new DownloadReceiver();
            receiver.DownloadCompleted += Receiver_DownloadCompleted;

            RegisterReceiver(receiver, filter);
        }

        private void Receiver_DownloadCompleted()
        {
            try
            {
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.N)
                {
                    Intent intent = new Intent(Intent.ActionView);
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.NewTask | ActivityFlags.SingleTop | ActivityFlags.ClearTop);
                    intent.PutExtra(Intent.ExtraNotUnknownSource, true);

                    var apkUri = Android.Support.V4.Content.FileProvider.GetUriForFile(
                                        this.ApplicationContext,
                                        string.Format("{0}{1}", Application.Context.PackageName, ".fileprovider"),
                                        new Java.IO.File(Android.OS.Environment.DirectoryDownloads + string.Format("{0}{1}{2}", "/", Application.Context.PackageName, ".apk")));

                    intent.SetDataAndType(apkUri, this.ContentResolver.GetType(apkUri));

                    StartActivity(intent);
                }
                else
                {
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetDataAndType(Android.Net.Uri.FromFile(
                                                new Java.IO.File(Android.OS.Environment.DirectoryDownloads + string.Format("{0}{1}{2}", "/", Application.Context.PackageName, ".apk")))
                                        , "application/vnd.android.package-archive");
                    intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.SingleTop | ActivityFlags.ClearTop); // ActivityFlags.NewTask 이 옵션을 지정해 주어야 업데이트 완료 후에 [열기]라는 화면이 나온다.
                    StartActivity(intent);
                }

                this.Finish();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();

            // Unregister the BroadcastReceiver when app is destroyed.
            if (receiver != null)
            {
                UnregisterReceiver(receiver);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnUpdateCompleted?.Invoke();
            OnUpdateCompleted = null;
        }

        private void FileDelete()
        {
            List<string> projection = new List<string>();
            projection.Add(MediaStore.Downloads.InterfaceConsts.Id);
            projection.Add(MediaStore.Downloads.InterfaceConsts.DisplayName);


            var query = ContentResolver.Query(MediaStore.Downloads.GetContentUri(MediaStore.VolumeExternal),
                            projection.ToArray(),
                            string.Format("{0} like '{1}%'", MediaStore.Downloads.InterfaceConsts.DisplayName, Application.Context.PackageName),
                            null,
                            null);

            var idColumn = query.GetColumnIndexOrThrow(MediaStore.Downloads.InterfaceConsts.Id);

            while (query != null && query.MoveToNext())
            {
                var id = query.GetLong(idColumn);
                var contentUri = ContentUris.WithAppendedId(MediaStore.Downloads.GetContentUri(MediaStore.VolumeExternal), id);
                ContentResolver.Delete(contentUri, null, null);
            }



            /*
            File file = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString());
            IFilenameFilter filter = new AutoUpdateFileFilter();
            File[] files = file.ListFiles(filter);

            if (files != null && files.Length > 0)
            {
                foreach (var item in files)
                {
                    if (item.IsFile)
                    {
                        if (item.Name.ToString().StartsWith(Application.Context.PackageName))
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
            */
        }


        private System.IO.Stream OpenStream(string path)
        {
            return new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None, 4095);
        }


        public delegate void DownloadCompletedEventHandler();
        private class DownloadReceiver : BroadcastReceiver
        {
            public event DownloadCompletedEventHandler DownloadCompleted;

            public DownloadReceiver()
            {

            }

            public override void OnReceive(Context context, Intent intent)
            {
                long id = intent.GetLongExtra(DownloadManager.ExtraDownloadId, -1);

                System.Console.WriteLine(string.Format("Received intent for {0}:\n", id));

                if (DownloadManager.ActionDownloadComplete.Equals(intent.Action))
                {
                    if (downloadId == id)
                    {
                        var manager = DownloadManager.FromContext(context);
                        var query = new DownloadManager.Query();
                        query.SetFilterById(id);
                        ICursor cursor = manager.InvokeQuery(query);
                        if (cursor.MoveToFirst())
                        {
                            // get the status
                            var columnIndex = cursor.GetColumnIndex(DownloadManager.ColumnStatus);
                            var status = (DownloadStatus)cursor.GetInt(columnIndex);

                            System.Console.WriteLine(string.Format("  Received status {0}\n", status));

                            if (status == Android.App.DownloadStatus.Successful)
                            {
                                DownloadCompleted?.Invoke();
                            }
                        }
                    }
                }
            }
        }


    }
    
}