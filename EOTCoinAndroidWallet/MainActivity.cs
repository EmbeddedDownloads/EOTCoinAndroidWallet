using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Support.V7.App;

namespace EOTCoinAndroidWallet
{
    [Activity(Label = "EOT Coin Android Wallet")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
           // EditText eotAddressText = FindViewById<EditText>(Resource.Id.textView1);
            TextView eotAddressText = FindViewById<TextView>(Resource.Id.EOTAddressText);
            Button generateNewWalletButton = FindViewById<Button>(Resource.Id.GenerateNewWalletButton);
            Button loginButton = FindViewById<Button>(Resource.Id.LoginButton);
            generateNewWalletButton.Click += (sender, e) =>
            {
                // Translate user’s alphanumeric phone number to numeric
                eotAddressText.Text = GenerateNewWallet("abcd");
              
            };

            loginButton.Click += (sender, e) =>
            {
               // var intent = new Intent(this, typeof(LoginActivity));

                // intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
                StartActivity(new Intent(Application.Context, typeof(LoginActivity)));
            };

           
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public string GenerateNewWallet(string Password)
        {

            string inputString = "adsjfhlsdajkfhlsdkajhfsdalkjhfkljdsahfklasdjfhkasdjhflkajsdfhkljdsahfklsdajhfkljsdahfewuiriqwerweqrukljfhsadkljhfewiur423875843956239487653o847653284765oiweuyuioyriewuyriequyreljdflkhdjsfhjksdafhlaksdjfheuwruiewirouqweyior34o785784iowqeyrsadfjhjkfhaskjdfh";//RandomCharsTextBox.Text;
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

