using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.Orders
{
    internal class SelectToppingsRecyclerAdapter : RecyclerView.Adapter
    {
        Action<OrderToppingTypes> _handleToppingSelected;
        readonly Dictionary<string, OrderToppingTypes> _toppings;
        List<OrderToppingTypes> _currentOrderToppings = new List<OrderToppingTypes>();

        Activity _activity;

        public SelectToppingsRecyclerAdapter(Activity activity, Action<OrderToppingTypes> handleToppingSelected)
        {
            _activity = activity;
            _handleToppingSelected = handleToppingSelected;

            _toppings = Enum.GetNames(typeof(OrderToppingTypes))
                .OrderBy(a => a).ToDictionary(a => a, b => (OrderToppingTypes)Enum.Parse(typeof(OrderToppingTypes), b));
        }

        public override int ItemCount => _toppings.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _toppings.ElementAt(position);

            var h = (SelectToppingsRecyclerViewHolder)holder;

            var cb = h.View.FindViewById<CheckBox>(Resource.Id.topping);
            cb.Text = item.Key;

            cb.Checked = _currentOrderToppings.Contains(item.Value);

            cb.Click -= Cb_Click;
            cb.Click += Cb_Click;
        }

        private void Cb_Click(object sender, EventArgs e)
        {
            var p = _activity.FindViewById<RecyclerView>(Resource.Id.selected_toppings);
            var r = (View)((CheckBox)sender).Parent;

            int position = p.GetChildAdapterPosition(r);

            _handleToppingSelected(_toppings.ElementAt(position).Value);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var linearLayout = (LinearLayout)_activity.LayoutInflater.Inflate(Resource.Layout.order_select_toppings, null);

            return new SelectToppingsRecyclerViewHolder(linearLayout);
        }

        internal void OrderItemSelected(List<OrderToppingTypes> selectedToppings)
        {
            _currentOrderToppings = selectedToppings;
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