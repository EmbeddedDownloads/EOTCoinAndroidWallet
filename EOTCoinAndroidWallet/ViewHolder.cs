using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace EOTCoinAndroidWallet
{
    public class ViewHolder : Java.Lang.Object
    {
       // public ImageView Photo { get; set; }
        public TextView Date { get; set; }
        public TextView Status { get; set; }
        public TextView Amount { get; set; }
        public TextView TransactionType { get; set; }
        public TextView Address { get; set; }
    }
}