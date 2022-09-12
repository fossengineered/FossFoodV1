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
using static Android.Views.View;

namespace FossFoodV1.Orders
{
    public class OrdersWithToppingsRecyclerAdapter : RecyclerView.Adapter
    {
        public List<OrderWithToppings> _items;
        Activity _activity;
        Action<OrderWithToppings, int> _onOrderItemSelected;

        public OrdersWithToppingsRecyclerAdapter(List<OrderWithToppings> items, Activity activity, Action<OrderWithToppings,int> onOrderItemSelected)
        {
            _items = items;
            _activity = activity;
            _onOrderItemSelected = onOrderItemSelected;
        }

        public override int ItemCount => _items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _items[position];
            var h = holder as OrdersWithToppingsRecyclerViewHolder;

            var t = h.View.FindViewById<TextView>(Resource.Id.cardOrderWithToppings_ItemType);
            t.Text = item.OrderItemType.Name;

            var l = h.View.FindViewById<ListView>(Resource.Id.order_item_toppings);

            var b = h.View.FindViewById<ImageView>(Resource.Id.delRowBtn);

            b.Click -= B_Click;
            b.Click += B_Click;

            h.View.Click -= View_Click;
            h.View.Click += View_Click;

            if (item.AvailableToppings.Count(a=>a.Selected) == 0)
            {
                l.Adapter = new ArrayAdapter(_activity, Resource.Layout.order_item_toppings, new List<string> { "no toppings" });
                l.RequestLayout();

                return;
            }

            l.Adapter = new ArrayAdapter(
                    _activity,
                    Resource.Layout.order_item_toppings,
                    item.AvailableToppings.Where(a=>a.Selected).Select(a => a.Name).ToArray());


            l.LayoutParameters.Height = CalculateHeight(l);
            l.RequestLayout();

        }

        private int CalculateHeight(ListView list)
        {

            int height = 0;

            for (int i = 0; i < list.Count; i++)
            {
                View childView = list.Adapter.GetView(i, null, list);
                childView.Measure(MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified), MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
                height += childView.MeasuredHeight;
            }

            //dividers height
            height += list.DividerHeight * list.Count;

            return height;
        }

        internal void UpdateItem(OrderWithToppings item, int curreOrderItemPosition)
        {
            _items[curreOrderItemPosition] = item;
            NotifyDataSetChanged();
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

            _onOrderItemSelected(_items[position], position);
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