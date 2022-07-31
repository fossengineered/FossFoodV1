using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.Orders
{
    internal class OrdersViewModels
    {
        Activity _activity;
        List<OrderWithToppings>  _recyclerViewData = new List<OrderWithToppings>();

        public OrdersViewModels(Activity activity)
        {
            _activity = activity;

            var recycler = activity.FindViewById<RecyclerView>(Resource.Id.order_items);

            var layoutManager = new LinearLayoutManager(activity) { Orientation = LinearLayoutManager.Vertical };
            recycler.SetLayoutManager(layoutManager);
            recycler.HasFixedSize = true;

            var adapter = new OrdersWithToppingsRecyclerAdapter(_recyclerViewData, activity);
            
            recycler.SetAdapter(adapter);

            var orderButton = activity.FindViewById<FloatingActionButton>(Resource.Id.btn_add_order_item);
            orderButton.Click += (a, b) => {

                ShowSelectItemDialog((itemId) => {
                    
                    var item = Enum.GetNames(typeof(OrderItemTypes)).OrderBy(x => x).ToArray()[itemId];

                    adapter.AddItem(new OrderWithToppings { OrderItemType = (OrderItemTypes)Enum.Parse(typeof(OrderItemTypes), item) });

                });

            };
        }

        public void ShowSelectItemDialog(Action<int> onItemSelected)
        {
            var dialogView = _activity.LayoutInflater.Inflate(Resource.Layout.order_select_item, null);
            AlertDialog alertDialog;

            using (var dialog = new AlertDialog.Builder(_activity))
            {
                dialog.SetTitle("Select an Item"); dialog.SetMessage("Select a Order Item");
                dialog.SetView(dialogView); dialog.SetNegativeButton("Cancel", (s, a) => { });

                alertDialog = dialog.Create();
            }

            var adapter = new ArrayAdapter(_activity, Android.Resource.Layout.SimpleListItem1, new OrdersEntity().GetOrderItemTypes());

            var lv = dialogView.FindViewById<ListView>(Resource.Id.order_items_to_select);
            lv.Adapter = adapter;

            lv.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => { 
                onItemSelected(e.Position);
                alertDialog.Hide();
            };

            alertDialog.Show();
        }
    }
}