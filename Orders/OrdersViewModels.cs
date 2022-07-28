using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
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

            var recycler = activity.FindViewById<RecyclerView>(Resource.Id.order_items);

            var layoutManager = new LinearLayoutManager(activity) { Orientation = LinearLayoutManager.Vertical };
            recycler.SetLayoutManager(layoutManager);
            recycler.HasFixedSize = true;

            var recyclerViewData = new List<OrderWithToppings>();

            var adapter = new OrdersWithToppingsRecyclerAdapter(recyclerViewData, activity);
            
            recycler.SetAdapter(adapter);
        }
    }
}