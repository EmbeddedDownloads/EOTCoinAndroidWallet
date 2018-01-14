using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using ZXing;
using ZXing.Mobile;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
//using ZXing;

namespace EOTCoinAndroidWallet
{
    [Activity(Label = "EOT Coin Wallet Dashboard")]
    public class DashboardActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DashboardActivity);
            TextView eotAddressText = FindViewById<TextView>(Resource.Id.EOTAddress);
            TextView eotBalanceText = FindViewById<TextView>(Resource.Id.BalanceText);
            Button sendEOTButton = FindViewById<Button>(Resource.Id.SendEOTButton);
            Button refreshButton = FindViewById<Button>(Resource.Id.RefreshButton);
            Button historyButton = FindViewById<Button>(Resource.Id.HistoryButton);
            Button exportWalletSeedButton = FindViewById<Button>(Resource.Id.ExportWalletButton);
            ImageView qrCodeImageView = FindViewById<ImageView>(Resource.Id.QRCodeImageView);
            //  ImageView qrCodeImageView = FindViewById<ImageView>(Resource.Id.QRCodeImageView);
            string eotAddress = Intent.GetStringExtra("EOTAddress") ?? "Data not available";
            eotAddressText.Text = eotAddress;
            //GenerateQRImage(100, 100, eotAddress);
            qrCodeImageView.SetImageBitmap(GetQRCode(eotAddress));
            eotBalanceText.Text = Utilities.GetAddressBalance(eotAddress).ToString() + " EOT";

            sendEOTButton.Click += (sender, e) =>
            {
                //var sendEOTActivity = new Intent(this, typeof(SendActivity));
               // dashboardActivity.PutExtra("EOTAddress", eotAddress);
               // StartActivity(sendEOTActivity);
                StartActivity(new Intent(Application.Context, typeof(SendActivity)));
            };

            exportWalletSeedButton.Click += (sender, e) =>
            {
                //var sendEOTActivity = new Intent(this, typeof(SendActivity));
                // dashboardActivity.PutExtra("EOTAddress", eotAddress);
                // StartActivity(sendEOTActivity);
                StartActivity(new Intent(Application.Context, typeof(ExportSeed)));
            };

            historyButton.Click += (sender, e) =>
            {
                //var sendEOTActivity = new Intent(this, typeof(SendActivity));
                // dashboardActivity.PutExtra("EOTAddress", eotAddress);
                // StartActivity(sendEOTActivity);
                var transactionHistoryActivity = new Intent(Application.Context, typeof(TransactionHistory));
                transactionHistoryActivity.PutExtra("EOTAddress", eotAddress);
                StartActivity(transactionHistoryActivity);
            };

            refreshButton.Click += (sender, e) =>
            {
               // Toast.MakeText(this.ApplicationContext, eotAddress, ToastLength.Short).Show();
                eotBalanceText.Text = Refresh(eotAddress);
                Toast.MakeText(this.ApplicationContext, eotAddress, ToastLength.Short).Show();
            };            
        }    

        private Bitmap GetQRCode(string address)
        {
            var writer = new BarcodeWriter()
            {
                Format = ZXing.BarcodeFormat.QR_CODE, //BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 600,
                    Width = 600
                }
            };
            return writer.Write(address);
        }

        private string Refresh(string address)
        {
            decimal balance = Utilities.GetAddressBalance(address);
            return (balance.ToString() + " EOT");
        }
    }
}