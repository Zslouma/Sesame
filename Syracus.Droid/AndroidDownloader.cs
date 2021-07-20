using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Droid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDownloader))]
namespace Syracuse.Mobitheque.Droid
{
    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        public string DownloadFile(string url, string folder)
        {
            //string pathToNewFolder = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath);
            //Console.WriteLine(pathToNewFolder);
            //Console.WriteLine(Directory.Exists(pathToNewFolder));
            //Console.WriteLine(File.Exists(pathToNewFolder));
            //var thisActivity = Forms.Context as Activity;
           
            //if (ActivityCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            //{
            //    ActivityCompat.RequestPermissions(thisActivity, new string[] { Manifest.Permission.WriteExternalStorage }, 1);
            //}
            //if (ActivityCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
            //{
            //    ActivityCompat.RequestPermissions(thisActivity, new string[] { Manifest.Permission.WriteExternalStorage }, 1);
            //}
            //try
            //{
            //    WebClient webClient = new WebClient();
            //    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            //    string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
            //    webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    if (OnFileDownloaded != null)
            //        OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            //}
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
        }

        public string GetPathStorage()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
            else
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
            }
        }
    }
}