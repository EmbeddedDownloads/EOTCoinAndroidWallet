using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace EOTCoinAndroidWallet
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           // Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
           // Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
            await Task.Delay(1000); // Simulate a bit of startup work.
                                    // Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
            string filename = "/wallet.eot";
            string path2 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            // Console.WriteLine(filename);
            string path = path2 + filename;
            if (File.Exists(path)) //if set up already 
            {
                StartActivity(new Intent(Application.Context, typeof(LoginActivity))); //password prompt to log in
            }
            else
            {
                StartActivity(new Intent(Application.Context, typeof(ImportOrGenerate))); //password prompt to log in
            }
            
        }
    }
}