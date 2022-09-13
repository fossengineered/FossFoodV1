using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using FossFoodV1.Food;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FossFoodV1.Orders
{
    internal class SelectToppingsRecyclerAdapter : RecyclerView.Adapter
    {
        Action<Topping> _handleToppingSelected;
        OrderWithToppings _currentOrder = new OrderWithToppings();

        Activity _activity;

        public SelectToppingsRecyclerAdapter(Activity activity, Action<Topping> handleToppingSelected)
        {
            _activity = activity;
            _handleToppingSelected = handleToppingSelected;
        }

        public override int ItemCount => _currentOrder.AvailableToppings.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _currentOrder.AvailableToppings.ElementAt(position);

            var h = (SelectToppingsRecyclerViewHolder)holder;

            var cb = h.View.FindViewById<CheckBox>(Resource.Id.topping);
            cb.Text = item.Name;

            cb.Checked = item.Selected;

            cb.Click -= Cb_Click;
            cb.Click += Cb_Click;

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            h.View.FindViewById<TextView>(Resource.Id.upcharge).Text = item.Charge == 0 ? "" : $"{item.Charge.ToString("C", nfi)}";
        }

        private void Cb_Click(object sender, EventArgs e)
        {
            var p = _activity.FindViewById<RecyclerView>(Resource.Id.selected_toppings);
            var r = (View)((CheckBox)sender).Parent;

            int position = p.GetChildAdapterPosition(r);

            _handleToppingSelected(_currentOrder.AvailableToppings.ElementAt(position));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var linearLayout = (LinearLayout)_activity.LayoutInflater.Inflate(Resource.Layout.order_select_toppings, null);

            return new SelectToppingsRecyclerViewHolder(linearLayout);
        }

        internal void OrderItemSelected(OrderWithToppings order)
        {
            _currentOrder =  order;
            NotifyDataSetChanged();
        }
    }

    internal class SelectToppingsRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout View { get; set; }
        public SelectToppingsRecyclerViewHolder(LinearLayout itemView) : base(itemView)
        {
            View = itemView;
        }
    }
}