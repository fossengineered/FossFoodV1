using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using FossFoodV1.OrderManager;
using FossFoodV1.Orders;
using FossFoodV1.ServiceDates;
using Google.Android.Material.FloatingActionButton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.Tickets
{
    public class TicketsFragment : Fragment
    {
        View _v;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        private void Init(View v)
        {
            var orderManager = new OrderManagerEntity(new ServiceDatesEntity().Current);

            v.FindViewById<FloatingActionButton>(Resource.Id.btn_add_order).Click += AddOrderClick;

            InitOrderRecycler(
                orderManager.OpenOrders,
                RowStatus.Open,
                OpenTicketSelected);

            //InitOrderRecycler(
            //    orderManager.CloseOrders,
            //    RowStatus.Closed,
            //    ClosedTicketSelected);
        }

        private OrderManagerOrderRecycler InitOrderRecycler(
            List<OrderManagerOrders> orders,
            RowStatus rowStatus,
            Action<int> onItemSelected)
        {
            var orderItemRecycler = _v.FindViewById<RecyclerView>(Resource.Id.recycler_tickets);

            var layoutManager = new GridLayoutManager(Context, 3);//new LinearLayoutManager(Context) { Orientation = LinearLayoutManager.Vertical };
            orderItemRecycler.SetLayoutManager(layoutManager);
            orderItemRecycler.HasFixedSize = true;

            var adapter = new OrderManagerOrderRecycler(Activity, orders, rowStatus, onItemSelected, () => { }, Resource.Id.recycler_tickets);

            orderItemRecycler.SetAdapter(adapter);
            return adapter;
        }

        private void OpenTicketSelected(int orderId)
        {
            var intent = new Intent(Activity, typeof(OrdersActivity));

            intent.PutExtra("order_id", orderId.ToString());

            Activity.StartActivity(intent);
        }

        private void ClosedTicketSelected(int orderId)
        {
            var intent = new Intent(Activity, typeof(OrdersActivity));

            intent.SetFlags(ActivityFlags.ClearTop);
            intent.PutExtra("order_id", orderId.ToString());

            Activity.StartActivity(intent);
        }

        private void AddOrderClick(object sender, EventArgs e)
        {
            var intent = new Intent(Activity, typeof(OrdersActivity));
            intent.PutExtra("order_id", "");
            Activity.StartActivity(intent);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout._placeholder_tickets, container, false);

            _v = v;

            Init(v);

            return v;
        }
    }
}