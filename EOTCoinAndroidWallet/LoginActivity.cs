using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace EOTCoinAndroidWallet
{
    [Activity(Label = "EOT Coin Wallet Login", MainLauncher = false)]
    public class LoginActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginActivity);
            EditText passwordText = FindViewById<EditText>(Resource.Id.LoginPassword);
            Button loginButton = FindViewById<Button>(Resource.Id.LoginButton);
            TextView loginTextResult = FindViewById<TextView>(Resource.Id.LoginTextResult);
           // Button deleteWalletButtion = FindViewById<Button>(Resource.Id.DeleteWalletButton);
            // Create your application here
            loginButton.Click += (sender, e) =>
            {
                string eotAddress = "";
                eotAddress = LoginButton_Click(passwordText.Text);
                if ( eotAddress != "")
                {
                    var dashboardActivity = new Intent(this, typeof(DashboardActivity));
                    dashboardActivity.PutExtra("EOTAddress", eotAddress);
                    StartActivity(dashboardActivity);
                }
                else
                {
                    loginTextResult.Text = "Login Failed! Please try again.";
                }
                
            };

          /*  deleteWalletButtion.Click += (sender, e) =>
            {
                string path2 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                File.Delete(path2 + "/wallet.eot");
                File.Delete(path2 + "/Address.txt");
            };*/
        }


        private string LoginButton_Click(string pwd)
        {
            TextView loginTextResult = FindViewById<TextView>(Resource.Id.LoginTextResult);
            try
            {
                string password = pwd;
                string path2 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string seed = Utilities.FileDecrypt(path2+"/wallet.eot", password);
                string txtAddress = "";
                string filename = path2+"/Address.txt";
                string path = @"" + filename;
                if (File.Exists(path))
                {
                    txtAddress = File.ReadLines(path).First();
                }
                else
                {
               //     System.Windows.Forms.MessageBox.Show("Address.txt does not exist!");
                }
                //generate address using seed
                List<string> keyPair = Utilities.getKeyPair(seed);
                string eotPrivateKey = keyPair.ElementAt(0);
                string eotAddress = keyPair.ElementAt(1);
                Wallet eotWallet = new Wallet(eotPrivateKey, eotAddress, seed);
                //if address matches address.txt, go to dashboard
                if (txtAddress == eotWallet.eotAddress)
                {
                    //  EOTCoinWalletDashboard dashboardform = new EOTCoinWalletDashboard();
                    //  dashboardform.Show();
                    //  this.Hide();
                    loginTextResult.Text = eotAddress;
                    return eotAddress;
                    

                }
                else
                {
                    //  System.Windows.Forms.MessageBox.Show("Incorrect password, please try again!");
                    return ""; 
                }
            }
            catch (Exception ex)
            {
                // System.Windows.Forms.MessageBox.Show("Login failed: " + ex.Message);
                return ""; 
            }
        }
    }
}