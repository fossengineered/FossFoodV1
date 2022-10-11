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
        public OrderManagerViewModel(DateTime serviceDate, Activity activity)
        {
            _activity = activity;
            activity.FindViewById<TextView>(Resource.Id.current_service_date).Text = serviceDate.ToShortDateString();

            Init();
        }

        internal void Init()
        {
            var orderManager = new OrderManagerEntity(new ServiceDatesEntity().Current);

            _activity.FindViewById<FloatingActionButton>(Resource.Id.btn_add_order).Click += OrderManagerViewModel_AddOrder_Click; ;

            //orderManager.OpenOrders.Add(new Orders.OrderWithToppings());

            InitOrderRecycler(_activity, orderManager.OpenOrders, Resource.Id.recycler_open_orders);
            InitOrderRecycler(_activity, orderManager.CloseOrders, Resource.Id.recycler_closed_orders);
        }

        private void OrderManagerViewModel_AddOrder_Click(object sender, EventArgs e)
        {
            //Toast.MakeText(_activity.ApplicationContext, "Click", ToastLength.Short).Show();
            var intent = new Intent(_activity, typeof(OrdersActivity));
            intent.SetFlags(ActivityFlags.ClearTop);
            _activity.StartActivity(intent);
        }

        private OrderManagerOrderRecycler InitOrderRecycler(Activity activity, List<OrderWithToppings> orders, int recyclerId)
        {
            var orderItemRecycler = activity.FindViewById<RecyclerView>(recyclerId);

            var layoutManager = new LinearLayoutManager(activity) { Orientation = LinearLayoutManager.Vertical };
            orderItemRecycler.SetLayoutManager(layoutManager);
            orderItemRecycler.HasFixedSize = true;

            var adapter = new OrderManagerOrderRecycler(activity, orders);

            orderItemRecycler.SetAdapter(adapter);
            return adapter;
        }
    }
}