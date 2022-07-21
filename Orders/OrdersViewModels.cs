using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.Orders
{
    internal class OrdersViewModels
    {
        public OrdersViewModels(Activity activity)
        {
            var orderButton = activity.FindViewById<AppCompatButton>(Resource.Id.btn_add_order_item);
            orderButton.Click += (a, b) => { activity.StartActivity(new Intent(activity, typeof(FoodActivity))); };

            var orderItems = activity.FindViewById<ListView>(Resource.Id.order_items);
            var items = new List<string> { "1", "two" };
            orderItems.Adapter = new ArrayAdapter(activity, Resource.Layout.order_item, items);
        }
    }
}