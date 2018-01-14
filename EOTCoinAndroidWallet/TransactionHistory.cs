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
    [Activity(Label = "TransactionHistory")]
    public class TransactionHistory : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TransactionHistory);
            TableLayout transactionHistoryTable = FindViewById<TableLayout>(Resource.Id.TransactionTableLayout);
            ListView transactionhistoryList = FindViewById<ListView>(Resource.Id.listView1);
            string eotAddress = Intent.GetStringExtra("EOTAddress") ?? "Data not available";
            List<TransactionHistoryEOT> listtransaction = Utilities.GetEOTTransactionInfo(eotAddress);
            //Toast.MakeText(this.ApplicationContext, listtransaction.Count.ToString(), ToastLength.Short).Show();
            if (listtransaction != null)
            {
                transactionhistoryList.Adapter = new TransactionAdapter(listtransaction);
            }
            else
            {
                Toast.MakeText(this.ApplicationContext, "No Transactions Yet", ToastLength.Short).Show();
            }

        }
    }
}