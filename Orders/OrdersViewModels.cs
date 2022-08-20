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
        SelectToppingsRecyclerAdapter _toppingAdapter;
        OrdersWithToppingsRecyclerAdapter _orderItemAdapter;
        int _curreOrderItemPosition;


        public OrdersViewModels(Activity activity)
        {
            _activity = activity;

            _orderItemAdapter = InitOrderRecycler(activity);

            InitOrderBtn(activity, _orderItemAdapter);
        }

        private SelectToppingsRecyclerAdapter InitToppingRecycler(Activity activity)
        {
            var toppingRecycler = activity.FindViewById<RecyclerView>(Resource.Id.selected_toppings);
            var layoutManager = new LinearLayoutManager(activity) { Orientation = LinearLayoutManager.Vertical };
            toppingRecycler.SetLayoutManager(layoutManager);
            toppingRecycler.HasFixedSize = true;

            var adapter = new SelectToppingsRecyclerAdapter(activity, (topping) => {
                var item = _orderItemAdapter._items[_curreOrderItemPosition];

                if (item.Toppings.Contains(topping))
                    item.Toppings = item.Toppings.Except(new List<OrderToppingTypes> { topping }).ToList();
                else
                    item.Toppings.Add(topping);

                _orderItemAdapter.UpdateItem(item, _curreOrderItemPosition);
            });
            toppingRecycler.SetAdapter(adapter);
            return adapter;
        }

        private void InitOrderBtn(Activity activity, OrdersWithToppingsRecyclerAdapter adapter)
        {
            var orderButton = activity.FindViewById<FloatingActionButton>(Resource.Id.btn_add_order_item);
            orderButton.Click += (a, b) =>
            {

                ShowSelectItemDialog((itemId) =>
                {

                    var item = Enum.GetNames(typeof(OrderItemTypes)).OrderBy(x => x).ToArray()[itemId];

                    var order = new OrderWithToppings { 
                        OrderItemType = (OrderItemTypes)Enum.Parse(typeof(OrderItemTypes), item),
                        Toppings = new List<OrderToppingTypes>()
                    };

                    adapter.AddItem(order);

                    if(_toppingAdapter == null)
                        _toppingAdapter = InitToppingRecycler(activity);

                    _toppingAdapter.OrderItemSelected(order.Toppings);
                    _curreOrderItemPosition = 0;
                });

            };
        }

        private OrdersWithToppingsRecyclerAdapter InitOrderRecycler(Activity activity)
        {
            var orderItemRecycler = activity.FindViewById<RecyclerView>(Resource.Id.order_items);

            var layoutManager = new LinearLayoutManager(activity) { Orientation = LinearLayoutManager.Vertical };
            orderItemRecycler.SetLayoutManager(layoutManager);
            orderItemRecycler.HasFixedSize = true;

            var adapter = new OrdersWithToppingsRecyclerAdapter(_recyclerViewData, activity, HandleOrderItemSelected);

            orderItemRecycler.SetAdapter(adapter);
            return adapter;
        }

        void HandleOrderItemSelected(OrderWithToppings selectedOrderItem, int position)
        {

            _toppingAdapter.OrderItemSelected(selectedOrderItem.Toppings);
            _curreOrderItemPosition = position;

            //Toast toast = Toast.MakeText(_activity.ApplicationContext, $"toppings", ToastLength.Short);
            //toast.Show();
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