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
    [Activity(Label = "Generate New EOT Wallet")]
    public class GenerateNewWallet : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GenerateNewWallet);
            TextView newPassword = FindViewById<TextView>(Resource.Id.newPassword);
            TextView repeatPassword = FindViewById<TextView>(Resource.Id.repeatPassword);
            TextView randomChars = FindViewById<TextView>(Resource.Id.RandomChars);
            Button generateNewWalletButton = FindViewById<Button>(Resource.Id.GenerateNewWalletButton);

            generateNewWalletButton.Click += (sender, e) =>

            {
                if (newPassword.Text != "" && newPassword != null)
                {
                    if (randomChars.Text.Length > 120)
                    {
                        if (newPassword.Text == repeatPassword.Text)
                        {
                            string eotAddress = GenerateWallet(newPassword.Text, randomChars.Text);
                            var loginActivity = new Intent(this, typeof(LoginActivity));
                           // dashboardActivity.PutExtra("EOTAddress", eotAddress);
                           // StartActivity(dashboardActivity);
                            StartActivity(new Intent(Application.Context, typeof(LoginActivity)));
                        }
                        else
                        {
                            Toast.MakeText(this.ApplicationContext, "Passwords do not match", ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(this.ApplicationContext, "Please ensure that you have typed 120 characters", ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this.ApplicationContext, "Please enter a Password", ToastLength.Short).Show();
                }                
            };
        }

        public string GenerateWallet(string Password, string randomchars)
        {

            string inputString = randomchars;//RandomCharsTextBox.Text;
            Random rnd = new Random();
            string seed = Utilities.generateSeed(inputString, rnd);
            List<string> keyPair = Utilities.getKeyPair(seed);
            string eotPrivateKey = keyPair.ElementAt(0);
            string eotAddress = keyPair.ElementAt(1);
            Wallet eotWallet = new Wallet(eotPrivateKey, eotAddress, seed);
            Utilities.FileEncrypt(seed, Password);
            Utilities.printWalletToFile(eotWallet);
            return eotAddress;
            //EOTCoinWalletDashboard dashboardform = new EOTCoinWalletDashboard();
            // dashboardform.Show();
            //   this.Hide();
        }
    }
}