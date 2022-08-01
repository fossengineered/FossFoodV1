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
        Action<List<OrderToppingTypes>> _showToppingsCallback;

        public OrdersWithToppingsRecyclerAdapter(List<OrderWithToppings> items, Activity activity, Action<List<OrderToppingTypes>> showToppingsCallback)
        {
            _items = items;
            _activity = activity;
            _showToppingsCallback = showToppingsCallback;
        }

        public override int ItemCount => _items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _items[position];
            var h = holder as OrdersWithToppingsRecyclerViewHolder;

            var t = h.View.FindViewById<TextView>(Resource.Id.cardOrderWithToppings_ItemType);
            t.Text = item.OrderItemType.ToString().Replace("_", " ");

            var l = h.View.FindViewById<ListView>(Resource.Id.order_item_toppings);

            if (item.Toppings == null || item.Toppings.Count == 0)
                l.Adapter = new ArrayAdapter(_activity, Resource.Layout.order_item_toppings, new List<string> { "no toppings" });
            else
                l.Adapter = new ArrayAdapter(
                    _activity,
                    Resource.Layout.order_item_toppings,
                    item.Toppings.Select(a=>a.ToString().Replace("_", " ")).ToArray());

            var b = h.View.FindViewById<ImageView>(Resource.Id.delRowBtn);            

            b.Click -= B_Click;
            b.Click += B_Click;

            h.View.Click -= View_Click;
            h.View.Click += View_Click;
        }

        private void B_Click(object sender, EventArgs e)
        {
            var p = _activity.FindViewById<RecyclerView>(Resource.Id.order_items);
            var r = (View)((View)((ImageView)sender).Parent).Parent;

            int position = p.GetChildAdapterPosition(r);

            RemoveItem(position);  

            Toast toast = Toast.MakeText(_activity.ApplicationContext, $"delete: {position}", ToastLength.Short);
            toast.Show();
        }

        private void View_Click(object sender, EventArgs e)
        {
            int position = _activity.FindViewById<RecyclerView>(Resource.Id.order_items).GetChildAdapterPosition((View)sender);

            _showToppingsCallback(_items[position].Toppings ?? new List<OrderToppingTypes>());
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var cardView = (CardView)_activity.LayoutInflater.Inflate(Resource.Layout.order_item,null);

            return new OrdersWithToppingsRecyclerViewHolder(cardView);
        }

        public void AddItem(OrderWithToppings item)
        {
            _items = _items.Prepend(item).ToList();
            NotifyDataSetChanged();
        }

        public void RemoveItem(int position)
        {
            _items.RemoveAt(position);
            NotifyDataSetChanged();
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