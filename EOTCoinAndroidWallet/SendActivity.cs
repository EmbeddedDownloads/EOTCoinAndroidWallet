using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBitcoin;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZXing.Mobile;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using NBitcoin.DataEncoders;
using System.IO;
using Android.Support.V7.App;

namespace EOTCoinAndroidWallet
{
    [Activity(Label = "Send EOT")]
    public class SendActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SendActivity);
            Button scanButton = FindViewById<Button>(Resource.Id.ScanButton);
            Button sendEOTButton = FindViewById<Button>(Resource.Id.SendEOTButton);
            TextView receiverAddress = FindViewById<TextView>(Resource.Id.ReceiverAddress);
            TextView AmountEOTtextbox = FindViewById<TextView>(Resource.Id.AmountEOT);
            TextView MinerFeesTextBox = FindViewById<TextView>(Resource.Id.MiningFee);
            TextView PasswordTextBox = FindViewById<TextView>(Resource.Id.SendEOTPasswordText);
            MobileBarcodeScanner.Initialize(Application);

            scanButton.Click += async (sender, e) => {
                
                    var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                    var result = await scanner.Scan();

                if (result != null)
                {
                    receiverAddress.Text = result.Text;
                }
                   // Console.WriteLine("Scanned Barcode: " + result.Text);

            };

            sendEOTButton.Click += (sender, e) =>
            {
                if (receiverAddress.Text != "" && receiverAddress != null)
                {
                    if (AmountEOTtextbox.Text != "" && AmountEOTtextbox.Text != " " && AmountEOTtextbox.Text != null) //check if amount and miner fees are numeric
                    {
                        if (MinerFeesTextBox.Text != "" && MinerFeesTextBox.Text != null)
                        {
                            if (PasswordTextBox.Text != "" && PasswordTextBox.Text != null)
                            {
                                Send(PasswordTextBox.Text, receiverAddress.Text, AmountEOTtextbox.Text, MinerFeesTextBox.Text);
                            }
                        }
                        else
                        {
                            Toast.MakeText(this.ApplicationContext, "You must enter an amount of miner fees to send!", ToastLength.Short).Show();
                            //System.Windows.Forms.MessageBox.Show("You must enter an amount of miner fees to send!");
                        }
                    }
                    else
                    {
                        Toast.MakeText(this.ApplicationContext, "You must enter an amount of EOT coins to send!", ToastLength.Short).Show();
                        //System.Windows.Forms.MessageBox.Show("You must enter an amount of EOT coins to send!");
                    }
                }
                else
                {
                    Toast.MakeText(this.ApplicationContext, "Receiving address must not be empty!", ToastLength.Short).Show();
                    //System.Windows.Forms.MessageBox.Show("Receiving address must not be empty!");

                }            
            };
        } 
        
        public void Send(string password, string receivingaddress, string amounttext, string minerfeetext)
        {
            //string password = PasswordTextBox.Text
            string path2 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
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
                Toast.MakeText(this.ApplicationContext, "Address.txt does not exist!", ToastLength.Short).Show();
            }
            string seed = Utilities.FileDecrypt(path2+ "/wallet.eot", password);
            List<string> keyPair = Utilities.getKeyPair(seed);
            string eotPrivateKey = keyPair.ElementAt(0);
            string eotAddress = keyPair.ElementAt(1);
            Wallet eotWallet = new Wallet(eotPrivateKey, eotAddress, seed);
            if (eotWallet.eotAddress == txtAddress)//SendingAddressTextBox.Text) //password matches
            {
                string receivingAddress = receivingaddress;//ReceivingAddressTextBox.Text;
                decimal amount = Convert.ToDecimal(amounttext);//AmountEOTtextbox.Text);
                double minerFees = Convert.ToDouble(minerfeetext);//MinerFeesTextBox.Text);
                string vendorAddress = "EerE2Vp8M4RAypufWsDZ3UHvQphHy99R1Q"; //merchant address 
                decimal transactionFees = 0.000m; //no transaction fee applied
                                                  //check balance first
                if (Utilities.GetAddressBalance(eotWallet.eotAddress) >= (amount + Convert.ToDecimal(minerFees) + transactionFees))
                {
                    bool success = PerformEOTTransaction(eotWallet.eotPrivateKey, eotWallet.eotAddress, vendorAddress, transactionFees, receivingAddress, amount, minerFees);
                    if (success)
                    {
                        //System.Windows.Forms.MessageBox.Show("Transaction successfully processed!");
                        Toast.MakeText(this.ApplicationContext, "Transaction successfully processed!", ToastLength.Short).Show();
                        var dashboardActivity = new Intent(this, typeof(DashboardActivity));
                        dashboardActivity.PutExtra("EOTAddress", eotAddress);
                        StartActivity(dashboardActivity);
                    }
                    else
                    {
                        //System.Windows.Forms.MessageBox.Show("Transaction failed: Not enough funds to process transaction!");
                        Toast.MakeText(this.ApplicationContext, "Transaction failed: Not enough funds to process transaction!", ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this.ApplicationContext, "Not enough funds to process transaction!", ToastLength.Short).Show();
                    //System.Windows.Forms.MessageBox.Show("Not enough funds to process transaction!");
                }
            }
            else
            {
                Toast.MakeText(this.ApplicationContext, "Incorrect password!", ToastLength.Short).Show();
                //System.Windows.Forms.MessageBox.Show("Incorrect password!");
            }
        }

        public static bool PerformEOTTransaction(string bitcoinSecret, string senderAddress, string eotVendorAddress, decimal transactionFees, string receiverAddress, decimal amount, double minerfees)
        {
            bool result = false;
            try
            {
                //Transacion Builder handles the confirmations
                var me = BitcoinAddress.Create(senderAddress);
                TransactionBuilder builder = new TransactionBuilder();
                List<Coin> coins;
                coins = GetCoinList(senderAddress);
                Transaction transaction =
                    builder
                    .AddCoins(coins)
                    .AddKeys(new BitcoinSecret(bitcoinSecret))
                    .Send(BitcoinAddress.Create(receiverAddress), Money.Coins(amount))
                    .Send(BitcoinAddress.Create(eotVendorAddress), Money.Coins(transactionFees))
                    .SendFees(Money.Coins(Convert.ToDecimal(minerfees)))
                    .SetChange(me).BuildTransaction(true);
                result = PushTransaction(transaction.ToHex());
                return result;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Transaction Failed -- Colm testing: " + ex.Message);
                //Console.ReadKey();
            }
            return result;
        }

        public static List<Coin> GetCoinList(string address)
        {
            try
            {
                string ApiBaseAddressForEOT = "http://178.62.30.50:3622/api";
                string URL = ApiBaseAddressForEOT + "/addr/" + address + "/utxo";
                List<Coin> coins = new List<Coin>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response1 = client.GetAsync(URL).Result;  // Blocking call!
                if (response1.IsSuccessStatusCode)
                {
                    var dataObjects = response1.Content.ReadAsStringAsync().Result;
                    var json = JArray.Parse(dataObjects);
                    foreach (var li in json)
                    {
                        {
                            // string txids = li["txid"].ToString();
                            // uint256 txidu = uint256(txids);
                            UInt32 voutu = Convert.ToUInt32(li["vout"].ToString());
                            decimal satoshis = Convert.ToDecimal(li["amount"].ToString()) * 100000000;
                            //  string scripts = Script(Encoders.Hex.DecodeData(li["scriptPubKey"].ToString();
                            //var coin = new Coin(new txidu, voutu, satoshis, new scripts)));
                            var coin = new Coin(new
                                                uint256(li["txid"].ToString()), Convert.ToUInt32(li["vout"].ToString()), Money.Satoshis(satoshis), new
                            Script(Encoders.Hex.DecodeData(li["scriptPubKey"].ToString())));
                            coins.Add(coin);
                        }
                    }
                    return coins;
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Exception in GetCoinListUsingPrimaryAPI method, Message : " + ex.Message);
            }
            return null;
        }

        public static bool PushTransaction(string hex)
        {
            try
            {
                HttpClient client = new HttpClient();
                string ApiBaseAddressForEOT = "http://178.62.30.50:3622/api";
                string URL = ApiBaseAddressForEOT + "/tx/send";
                var tx = "{\"rawtx\":" + "\"" + hex + "\"}";
                string _ContentType = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));
                HttpContent _Body = new StringContent(tx);
                // and add the header to this object instance
                // optional: add a formatter option to it as well
                _Body.Headers.ContentType = new MediaTypeHeaderValue(_ContentType);
                // synchronous request without the need for .ContinueWith() or await
                var response = client.PostAsync(new Uri(URL), _Body).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.IsSuccessStatusCode;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Exception in PushTransaction method. :" + ex.ToString());
                return false;
            }
        }
    }
}