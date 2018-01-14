using System;
using System.Collections.Generic;
using System.IO;
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
    [Activity(Label = "Export Wallet Seed")]
    public class ExportSeed : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ExportSeed);
            Button exportWalletSeed = FindViewById<Button>(Resource.Id.ExportSeedButton);
            TextView seedText = FindViewById<TextView>(Resource.Id.WalletSeedText);
            TextView passwordSeed = FindViewById<TextView>(Resource.Id.PasswordSeed);

            exportWalletSeed.Click += (sender, e) =>
            {
                string result = ExportButton_Click(passwordSeed.Text);
                if (result == "Incorrect password!" )
                {
                    Toast.MakeText(this.ApplicationContext, result, ToastLength.Short).Show();
                }
                else
                {
                    seedText.Text = result;
                }

            };

            // Create your application here
        }

        private string ExportButton_Click(string password)
        {
            string path2 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string eotSeed = Utilities.FileDecrypt(path2 + "/wallet.eot", password);
            string txtAddress = "";
            string filename = path2 + "/Address.txt";
            string path = @"" + filename;
            if (File.Exists(path))
            {
                txtAddress = File.ReadLines(path).First();
            }
            else
            {
                //     System.Windows.Forms.MessageBox.Show("Address.txt does not exist!");
            }

            List<string> keyPair = Utilities.getKeyPair(eotSeed);
            string eotPrivateKey = keyPair.ElementAt(0);
            string eotAddress = keyPair.ElementAt(1);
            Wallet eotWallet = new Wallet(eotPrivateKey, eotAddress, eotSeed);

            if (eotWallet.eotAddress == txtAddress)
            {
                return eotSeed;
            }
            else
            {
                return("Incorrect password!");
            }
        }
    }


}