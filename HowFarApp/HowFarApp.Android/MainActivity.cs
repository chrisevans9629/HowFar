﻿using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;
using Android.Media;
using HowFarApp.Views;
using Environment = System.Environment;

namespace HowFarApp.Droid
{
    [Activity(Label = "HowFarApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {

            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "exrin.db");
            LoadApplication(new App(dbPath, new LocationService()));
            //StartPlayer("Assets/bensound-creativeminds.mp3");
        }
        //protected MediaPlayer player;
        //public void StartPlayer(String filepath)
        //{
        //    if (player == null)
        //    {
        //        player = new MediaPlayer();
        //    }
        //    else
        //    {
        //        player.Reset();
        //        player.SetDataSource(filepath);
        //        player.Prepare();
        //        player.Start();
        //    }
        //}
    }
    public class LocationService : ILocationService
    {
        public bool LocationEnabled => true;
    }
}