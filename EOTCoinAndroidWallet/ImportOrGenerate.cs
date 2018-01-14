using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace EOTCoinAndroidWallet
{
    [Activity(Label = "Import Or Generate Wallet")]
    public class ImportOrGenerate : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ImportOrGenerate);
            Button generateNewWalletButton = FindViewById<Button>(Resource.Id.GenerateNewButton);
            Button importWalletButton = FindViewById<Button>(Resource.Id.ImportWalletButton);

            generateNewWalletButton.Click += (sender, e) =>
            {
                //var sendEOTActivity = new Intent(this, typeof(SendActivity));
                // dashboardActivity.PutExtra("EOTAddress", eotAddress);
                // StartActivity(sendEOTActivity);
                StartActivity(new Intent(Application.Context, typeof(GenerateNewWallet)));
            };

            importWalletButton.Click += (sender, e) =>
            {
                //var sendEOTActivity = new Intent(this, typeof(SendActivity));
                // dashboardActivity.PutExtra("EOTAddress", eotAddress);
                // StartActivity(sendEOTActivity);
                StartActivity(new Intent(Application.Context, typeof(ImportWallet)));
            };

            // Create your application here
        }
    }
}