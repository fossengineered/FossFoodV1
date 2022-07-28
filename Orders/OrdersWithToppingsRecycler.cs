using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.Orders
{
    public class OrdersWithToppingsRecyclerAdapter : RecyclerView.Adapter
    {
        List<OrderWithToppings> _items;
        Activity _activity;

        public OrdersWithToppingsRecyclerAdapter(List<OrderWithToppings> items, Activity activity)
        {
            _items = items;
            _activity = activity;
        }

        public override int ItemCount => _items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _items[position];
            var h = holder as OrdersWithToppingsRecyclerViewHolder;

            var t = h.View.FindViewById<TextView>(Resource.Id.cardOrderWithToppings_ItemType);
            t.Text = item.OrderItemType.ToString().Replace("_", " ");
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var cardView = (CardView)_activity.LayoutInflater.Inflate(Resource.Layout.order_item,null);

            return new OrdersWithToppingsRecyclerViewHolder(cardView);
        }

        public void AddItem(OrderWithToppings item)
        {
            _items.Add(item);
        }

        public void RemoveItem(int position)
        {
            _items.RemoveAt(position); 
        }
    }

    public class OrdersWithToppingsRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public CardView View { get; set; } 
        public OrdersWithToppingsRecyclerViewHolder(CardView itemView) : base(itemView)
        {
            View = itemView;
        }
    }
}