using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using FossFoodV1.Food;
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
        
        SelectToppingsRecyclerAdapter _toppingAdapter;
        OrdersWithToppingsRecyclerAdapter _orderItemAdapter;
        int _curreOrderItemPosition;

        public static string _customerName;
        public static int? _pagerNumber;
        public static List<OrderWithToppings> _ordersWithToppings = new List<OrderWithToppings>();

        public OrdersViewModels(Activity activity)
        {
            _activity = activity;

            _orderItemAdapter = InitOrderRecycler(activity);

            InitOrderBtn(activity, _orderItemAdapter);
            InitFormFields(activity);
            InitCompleteOrder(activity);
        }

        public void ClearData()
        {
            _customerName = null;
            _pagerNumber = null;
        }

        private void InitCompleteOrder(Activity activity)
        {
            activity.FindViewById<Button>(Resource.Id.btn_complete_order).Click += OrdersViewModels_Click_Complete_Order;
        }

        private void OrdersViewModels_Click_Complete_Order(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_customerName))
            {
                Toast.MakeText(_activity.ApplicationContext, "Please enter a customer name", ToastLength.Short).Show();
                return;

                //toast.Show();
            }

            if (_pagerNumber == null)
            {
                Toast.MakeText(_activity.ApplicationContext, "Please enter a pager number", ToastLength.Short).Show();
                return;

                //toast.Show();
            }

            if(_ordersWithToppings == null || _ordersWithToppings.Count == 0)
            {
                Toast.MakeText(_activity.ApplicationContext, "Please add at least 1 order item", ToastLength.Short).Show();
                return;

                //toast.Show();
            }

            var intent = new Intent(_activity.ApplicationContext, typeof(CompleteOrderActivity));
            _activity.StartActivity(intent);
        }

        private void InitFormFields(Activity activity)
        {
            activity.FindViewById<EditText>(Resource.Id.order_name).AfterTextChanged += OrdersViewModels_AfterTextChanged_Order_Name;

            activity.FindViewById<EditText>(Resource.Id.pager_num).AfterTextChanged += OrdersViewModels_AfterTextChanged_Pager_Num;
        }

        private void OrdersViewModels_AfterTextChanged_Pager_Num(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            var t = _activity.FindViewById<EditText>(Resource.Id.pager_num).Text;

            if (String.IsNullOrEmpty(t))
                return;

            int outNum;

            if (!int.TryParse(t, out outNum))
                return;

            _pagerNumber = outNum;
        }

        private void OrdersViewModels_AfterTextChanged_Order_Name(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            var t = _activity.FindViewById<EditText>(Resource.Id.order_name).Text;

            if (String.IsNullOrEmpty(t))
                return;

            _customerName = t;
        }

        private SelectToppingsRecyclerAdapter InitToppingRecycler(Activity activity)
        {
            var toppingRecycler = activity.FindViewById<RecyclerView>(Resource.Id.selected_toppings);
            var layoutManager = new LinearLayoutManager(activity) { Orientation = LinearLayoutManager.Vertical };
            toppingRecycler.SetLayoutManager(layoutManager);
            toppingRecycler.HasFixedSize = true;

            var adapter = new SelectToppingsRecyclerAdapter(activity, (topping) => {
                var item = _ordersWithToppings[_curreOrderItemPosition];

                var t = item.AvailableToppings.First(a => a.Name.Equals(topping.Name, StringComparison.OrdinalIgnoreCase));
                t.Selected = !t.Selected;

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

                ShowSelectItemDialog((foodItem) =>
                {
                    var foodEntity = new FoodEntity();
                    //var item = Enum.GetNames(typeof(OrderItemTypes)).OrderBy(x => x).ToArray()[itemId];
                    var orderItemType = foodEntity.MenuItems.Items.Single(a => a.Name.Equals(foodItem, StringComparison.OrdinalIgnoreCase));


                    var order = new OrderWithToppings {
                        OrderItemType = orderItemType,
                        AvailableToppings = foodEntity.GetAvailableToppings(orderItemType.Name)
                    };

                    adapter.AddItem(order);

                    if(_toppingAdapter == null)
                        _toppingAdapter = InitToppingRecycler(activity);

                    _toppingAdapter.OrderItemSelected(order);
                    _curreOrderItemPosition = 0;
                });

            };
        }

        private OrdersWithToppingsRecyclerAdapter InitOrderRecycler(Activity activity)
        {
            _ordersWithToppings = new List<OrderWithToppings>();
            var orderItemRecycler = activity.FindViewById<RecyclerView>(Resource.Id.order_items);

            var layoutManager = new LinearLayoutManager(activity) { Orientation = LinearLayoutManager.Vertical };
            orderItemRecycler.SetLayoutManager(layoutManager);
            orderItemRecycler.HasFixedSize = true;

            var adapter = new OrdersWithToppingsRecyclerAdapter(activity, HandleOrderItemSelected);

            orderItemRecycler.SetAdapter(adapter);
            return adapter;
        }

        void HandleOrderItemSelected(OrderWithToppings selectedOrderItem, int position)
        {

            _toppingAdapter.OrderItemSelected(selectedOrderItem);
            _curreOrderItemPosition = position;

            //Toast toast = Toast.MakeText(_activity.ApplicationContext, $"toppings", ToastLength.Short);
            //toast.Show();
        }

        public void ShowSelectItemDialog(Action<string> onItemSelected)
        {
            var dialogView = _activity.LayoutInflater.Inflate(Resource.Layout.order_select_item, null);
            AlertDialog alertDialog;

            using (var dialog = new AlertDialog.Builder(_activity))
            {
                dialog.SetTitle("Select an Item"); dialog.SetMessage("Select a Order Item");
                dialog.SetView(dialogView); dialog.SetNegativeButton("Cancel", (s, a) => { });

                alertDialog = dialog.Create();
            }

            var l = new FoodEntity().MenuItems;

            var adapter = new ArrayAdapter(
                _activity,
                Android.Resource.Layout.SimpleListItem1,
                new FoodEntity().MenuItems.Items.Select(a => a.Name).ToArray());

            var lv = dialogView.FindViewById<ListView>(Resource.Id.order_items_to_select);
            lv.Adapter = adapter;

            lv.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                onItemSelected(new FoodEntity().MenuItems.Items.ElementAt(e.Position).Name);
                alertDialog.Hide();
            };

            alertDialog.Show();
        }
    }
}