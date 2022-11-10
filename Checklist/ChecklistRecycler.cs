using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;
using FossFoodV1.OrderManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.Checklist
{
    internal class ChecklistRecycler:RecyclerView.Adapter
    {
        Activity _activity;
        List<ChecklistItem> _items;
        Action<int> _onItemSelected;

        public ChecklistRecycler(
            Activity activity,
            List<ChecklistItem> items,
            Action<int> onItemSelected)
        {
            _activity = activity;
            _items = items;
            onItemSelected = _onItemSelected;
        }

        public override int ItemCount => _items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = (LinearLayout)_activity.LayoutInflater.Inflate(Resource.Layout.checklist_item, null);

            return new ChecklistRecyclerViewHolder(view);
        }
    }

    public class ChecklistRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout View { get; set; }
        public ChecklistRecyclerViewHolder(LinearLayout itemView) : base(itemView)
        {
            View = itemView;
        }
    }
}