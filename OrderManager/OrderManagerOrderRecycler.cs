using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;
using FossFoodV1.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.OrderManager
{
    internal class OrderManagerOrderRecycler:RecyclerView.Adapter
    {
        Activity _activity;
        List<OrderWithToppings> _orders;

        public OrderManagerOrderRecycler(Activity activity, List<OrderWithToppings> orders)
        {
            _activity = activity;
            _orders = orders;
        }

        public override int ItemCount => _orders.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _orders[position];
            var h = holder as OrderManagerOrderRecyclerViewHolder;
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