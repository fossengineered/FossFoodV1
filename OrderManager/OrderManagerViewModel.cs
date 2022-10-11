using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using FossFoodV1.Orders;
using FossFoodV1.ServiceDates;
using Google.Android.Material.FloatingActionButton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.OrderManager
{
    internal class OrderManagerViewModel
    {
        Activity _activity;
        DateTime _serviceDate;
        public OrderManagerViewModel(DateTime serviceDate, Activity activity)
        {
            _activity = activity;
            _serviceDate = serviceDate;

            activity.FindViewById<TextView>(Resource.Id.current_service_date).Text = serviceDate.ToShortDateString();

            Init();
        }

        internal void Init()
        {
            var orderManager = new OrderManagerEntity(_serviceDate);

            _activity.FindViewById<FloatingActionButton>(Resource.Id.btn_add_order).Click += OrderManagerViewModel_AddOrder_Click; ;

            InitOrderRecycler(
                _activity, 
                orderManager.OpenOrders, 
                Resource.Id.recycler_open_orders, 
                RowStatus.Open,
                a => { },
                () => { _activity.Recreate(); });

            InitOrderRecycler(
                _activity, 
                orderManager.CloseOrders, 
                Resource.Id.recycler_closed_orders, 
                RowStatus.Closed,
                b => { },
                () => { _activity.Recreate(); });
        }

        private void OrderManagerViewModel_AddOrder_Click(object sender, EventArgs e)
        {
            //Toast.MakeText(_activity.ApplicationContext, "Click", ToastLength.Short).Show();
            var intent = new Intent(_activity, typeof(OrdersActivity));
            intent.SetFlags(ActivityFlags.ClearTop);
            _activity.StartActivity(intent);
        }

        private OrderManagerOrderRecycler InitOrderRecycler(
            Activity activity, 
            List<OrderManagerOrders> orders, 
            int recyclerId, 
            RowStatus rowStatus,
            Action<int> onItemSelected,
            Action refreshParent)
        {
            var orderItemRecycler = activity.FindViewById<RecyclerView>(recyclerId);

            var layoutManager = new LinearLayoutManager(activity) { Orientation = LinearLayoutManager.Vertical };
            orderItemRecycler.SetLayoutManager(layoutManager);
            orderItemRecycler.HasFixedSize = true;

            var adapter = new OrderManagerOrderRecycler(activity, orders, rowStatus, onItemSelected, refreshParent);

            orderItemRecycler.SetAdapter(adapter);
            return adapter;
        }
    }
}