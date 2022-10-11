using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;
using FossFoodV1.Orders;
using FossFoodV1.ServiceDates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.OrderManager
{
    internal class OrderManagerOrderRecycler : RecyclerView.Adapter
    {
        Activity _activity;
        List<OrderManagerOrders> _orders;
        RowStatus _rowStatus;
        Action<int> _onOrderSelected;
        Action _refreshParent;

        public OrderManagerOrderRecycler(
            Activity activity, 
            List<OrderManagerOrders> orders, 
            RowStatus rowStatus,
            Action<int> onOrderSelected,
            Action refreshParent)
        {
            _activity = activity;
            _orders = orders;
            _rowStatus = rowStatus;
            _onOrderSelected = onOrderSelected;
            _refreshParent = refreshParent;
        }

        public override int ItemCount => _orders.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _orders[position];
            var h = holder as OrderManagerOrderRecyclerViewHolder;

            h.View.FindViewById<TextView>(Resource.Id.order_id).Text = $"Order Number  {item.OrdersId}";
            h.View.FindViewById<TextView>(Resource.Id.customer_name).Text = item.CustomerName;
            h.View.FindViewById<TextView>(Resource.Id.created_on).Text = item.CreatedOn.ToShortTimeString();

            if (_rowStatus == RowStatus.Open)
            {
                var b = h.View.FindViewById<TextView>(Resource.Id.btn_order_manager_item);
                b.Text = "Close Ticket";

                b.Click -= CloseTicket_Click;
                b.Click += CloseTicket_Click;

                var i = h.View.FindViewById<TextView>(Resource.Id.order_id);

                i.Click -= ViewOpenOrder_Click;
                i.Click += ViewOpenOrder_Click;
            }
            else if (_rowStatus == RowStatus.Closed)
            {
                var b = h.View.FindViewById<TextView>(Resource.Id.btn_order_manager_item);
                b.Text = "Re-Open Ticket";

                b.Click -= ReOpenTicket_Click;
                b.Click += ReOpenTicket_Click;

                var i = h.View.FindViewById<TextView>(Resource.Id.order_id);

                i.Click -= ViewClosedOrder_Click;
                i.Click += ViewClosedOrder_Click;
            }
        }

        private void ViewClosedOrder_Click(object sender, EventArgs e)
        {
            var p = _activity.FindViewById<RecyclerView>(Resource.Id.recycler_closed_orders);
            var r = ((View)sender).Parent.Parent.Parent;

            int position = p.GetChildAdapterPosition((View)r);

            _onOrderSelected(_orders[position].OrdersId);
        }

        private void ViewOpenOrder_Click(object sender, EventArgs e)
        {
            var p = _activity.FindViewById<RecyclerView>(Resource.Id.recycler_open_orders);
            var r = ((View)sender).Parent.Parent.Parent;

            int position = p.GetChildAdapterPosition((View)r);

            _onOrderSelected(_orders[position].OrdersId);
        }

        private void CloseTicket_Click(object sender, EventArgs e)
        {
            var p = _activity.FindViewById<RecyclerView>(Resource.Id.recycler_open_orders);
            var r = ((View)((Button)sender).Parent).Parent;

            int position = p.GetChildAdapterPosition((View)r);

            new OrderManagerEntity(new ServiceDatesEntity().Current).CloseOrder(_orders[position].OrdersId);

            _refreshParent();

            NotifyDataSetChanged();
        }

        private void ReOpenTicket_Click(object sender, EventArgs e)
        {
            var p = _activity.FindViewById<RecyclerView>(Resource.Id.recycler_open_orders);
            var r = ((View)((Button)sender).Parent).Parent;

            int position = p.GetChildAdapterPosition((View)r);

            new OrderManagerEntity(new ServiceDatesEntity().Current).ReOpenOrder(_orders[position].OrdersId);

            _refreshParent();

            NotifyDataSetChanged();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var cardView = (CardView)_activity.LayoutInflater.Inflate(Resource.Layout.order_manager_item, null);

            return new OrderManagerOrderRecyclerViewHolder(cardView);
        }
    }

    public class OrderManagerOrderRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public CardView View { get; set; }
        public OrderManagerOrderRecyclerViewHolder(CardView itemView) : base(itemView)
        {
            View = itemView;
        }
    }
}