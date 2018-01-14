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
    [Activity(Label = "Import EOT Wallet")]
    public class ImportWallet : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ImportWallet);

            Button importWalletButton = FindViewById<Button>(Resource.Id.ImportWalletButton);
            TextView walletSeed = FindViewById<TextView>(Resource.Id.WalletSeed);
            TextView newpassword = FindViewById<TextView>(Resource.Id.importPassword);
            TextView passwordrepeat = FindViewById<TextView>(Resource.Id.RepeatImportPassword);
            importWalletButton.Click += (sender, e) =>
            {
                ImportButton_Click(walletSeed.Text, newpassword.Text, passwordrepeat.Text);
            };
        }

        private void ImportButton_Click(string walletseed, string newpassword, string passwordrepeat)
        {
            if (walletseed != "" && walletseed != null)
            {
                if (newpassword != "" && newpassword != null)
                {
                    if (newpassword == passwordrepeat)
                    {
                        import(walletseed, newpassword);
                    }
                    else
                    {
                        //passwords do not match
                        Toast.MakeText(this.ApplicationContext, "Passwords do not match", ToastLength.Short).Show();
                    }
                }
                else
                {
                   // System.Windows.Forms.MessageBox.Show("Please enter a password");
                    Toast.MakeText(this.ApplicationContext, "Please enter a password", ToastLength.Short).Show();
                }
            }
            else
            {
                // System.Windows.Forms.MessageBox.Show("Please enter your wallet seed");
                Toast.MakeText(this.ApplicationContext, "Please enter your wallet seed", ToastLength.Short).Show();
            }
        }



        public void import(string walletseed, string passwordnew)
        {
            try
            {
                string seed = walletseed;
                string password = passwordnew;
                List<string> keyPair = Utilities.getKeyPair(seed);
                string eotPrivateKey = keyPair.ElementAt(0);
                string eotAddress = keyPair.ElementAt(1);
                Wallet eotWallet = new Wallet(eotPrivateKey, eotAddress, seed);
                string path2 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string file1 = path2 + "/wallet.eot";
                string file2 = path2 + "/Address.txt";
                if (File.Exists(file1) && File.Exists(file2))
                {
                    File.Delete(file1);
                    File.Delete(file2);
                }
                Utilities.FileEncrypt(seed, password);
                Utilities.printWalletToFile(eotWallet);
                Toast.MakeText(this.ApplicationContext, "Wallet imported successfully!", ToastLength.Short).Show();
                StartActivity(new Intent(Application.Context, typeof(LoginActivity))); 


                //System.Windows.Forms.MessageBox.Show("Wallet imported successfully!");
                // EOTCoinWalletDashboard dashboard = new EOTCoinWalletDashboard();
                //dashboard.Show();
                // this.Hide();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this.ApplicationContext, "Import failed: " + ex.Message, ToastLength.Short).Show();
                //System.Windows.Forms.MessageBox.Show("Import failed: " + ex.Message);
            }

        }
    }
}