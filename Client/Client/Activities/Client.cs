﻿using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;

using Client.Utilities;

using System;

namespace Client.Activities
{
    [Activity(Label = "Client", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/ClientTheme")]
    public class Base : Activity
    {
        public const int PingCode = -1;
        public const int DisconnectionCode = -2;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Empty);

            if (CheckSelfPermission(Android.Manifest.Permission.WriteExternalStorage) == Android.Content.PM.Permission.Denied)
            {
                Request();
            }
            else
            {
                Initialize();
            }            
        }

        public override void OnRequestPermissionsResult(int code, string[] permissions, Permission[] results)
        {
            if (code == 1111)
            {
                foreach (var result in results)
                {
                    if (result != Permission.Granted)
                    {
                        OnDenied();
                        return;
                    }
                }

                Initialize();
            }
        }

        private void Request ()
        {
            RequestPermissions(new string[] { Android.Manifest.Permission.WriteExternalStorage, Android.Manifest.Permission.ReadExternalStorage }, 1111);
        }

        private void OnDenied ()
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle("Oikeudet");
            builder.SetMessage("Sovellus tarvitsee kaikkia äskettäin pyydettyjä oikeuksia toimiakseen!");
            builder.SetCancelable(false);
            builder.SetPositiveButton("Yritä uudelleen", (Sender, Arguments) => { Request(); });
            builder.SetNegativeButton("Poistu", (Sender, Arguments) => { Finish(); });
            builder.Show(); 
        }

        private void OnStartupError (Exception e)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle("Virhe");
            builder.SetMessage("Käynnistäminen epäonnistui: " + e.ToString());
            builder.SetCancelable(false);
            builder.SetPositiveButton("OK", (Sender, Arguments) => { Finish(); });
            builder.Show();
        }

        private void Initialize ()
        {
            SetContentView(Resource.Layout.Wait);

            try
            {
                EditorActivity.Initialize(this);
                Cryptography.Initialize();
                Connection.Initialize(this);
            }
            catch (Exception e)
            {
                OnStartupError(e);
            }           
        }
    }
}

