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

namespace EOTCoinAndroidWallet
{
    class TransactionAdapter : BaseAdapter<TransactionHistoryEOT>
    {
        List<TransactionHistoryEOT> transactionHistoryEOT;

        public TransactionAdapter(List<TransactionHistoryEOT> transactionHistoryEOT)
        {
            this.transactionHistoryEOT = transactionHistoryEOT;
           
        }

        public override TransactionHistoryEOT this[int position]
        {
            get
            {
                return transactionHistoryEOT[position];
            }
        }

      //  public override TransactionHistoryEOT this[int position] => throw new NotImplementedException();

        public override int Count
        {
            get
            {
                return transactionHistoryEOT.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.transactionRow, parent, false);

                var date = view.FindViewById<TextView>(Resource.Id.DateTextView);
                var status = view.FindViewById<TextView>(Resource.Id.statusTextView);
                var amount = view.FindViewById<TextView>(Resource.Id.AmountTextView);
                var transactiontype = view.FindViewById<TextView>(Resource.Id.transactionTypeTextView);
                var address = view.FindViewById<TextView>(Resource.Id.AddressTextView);

                view.Tag = new ViewHolder() { Date = date, Status = status, Amount = amount, TransactionType = transactiontype, Address = address };
            }

            var holder = (ViewHolder)view.Tag;

            // holder.Photo.SetImageDrawable(ImageManager.Get(parent.Context, transactionHistoryEOT[position].ImageUrl));
            holder.Date.Text = transactionHistoryEOT[position].Date.ToString();
            holder.Status.Text = transactionHistoryEOT[position].TxStatus;
            holder.Amount.Text = transactionHistoryEOT[position].Amount.ToString();
            if (transactionHistoryEOT[position].Type == "0")
            {
                holder.TransactionType.Text = "Sent";
                holder.Address.Text = transactionHistoryEOT[position].Address[0];
            }
            else
            {
                holder.TransactionType.Text = "Received";
                holder.Address.Text = transactionHistoryEOT[position].Address[0];
            }
            
                
            return view;

        }
    }
}