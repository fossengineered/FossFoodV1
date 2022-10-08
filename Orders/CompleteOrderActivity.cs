using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using XFBluetoothPrint.Droid;

namespace FossFoodV1.Orders
{
    [Activity(Label = "CompleteOrderActivity", Theme = "@style/AppTheme.NoActionBar")]
    public class CompleteOrderActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.order_complete_order);

            FindViewById<Button>(Resource.Id.btn_complete_order).Click += CompleteOrderActivity_Click;
            FindViewById<Button>(Resource.Id.btn_print_order).Click += PrintOrder_Click; 

            SetOrderTotal();
        }

        private void PrintOrder_Click(object sender, EventArgs e)
        {
            var btPrint = new AndroidBlueToothService();

            var device = btPrint.GetDeviceList();

            if (device == null || device.Count == 0)
            {
                Toast.MakeText(ApplicationContext, "No printers found", ToastLength.Short).Show();
                return;
            }

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            OrdersViewModels._ordersWithToppings.ForEach(order =>
            {
                btPrint.Print(device[0], $"{order.OrderItemType.Name} {order.OrderItemType.BasePrice}");
                order.AvailableToppings.Where(t => t.Selected).ToList().ForEach(topping =>
                {
                    var charge = topping.Charge != 0 ? topping.Charge.ToString("C", nfi) : "";
                    btPrint.Print(device[0], $"{topping.Name} {charge}");
                });
            });
        }

        private void SetOrderTotal()
        {
            var baseAmount = OrdersViewModels._ordersWithToppings.Sum(a => a.OrderItemType.BasePrice);
            OrdersViewModels._ordersWithToppings.ForEach(a =>
            {
                baseAmount += a.AvailableToppings.Where(b => b.Selected).Select(c => c.Charge).Sum();
            });

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            FindViewById<TextView>(Resource.Id.amount_due).Text = $"{baseAmount.ToString("C", nfi)}";
        }

        private void CompleteOrderActivity_Click(object sender, EventArgs e)
        {
            var btPrint = new AndroidBlueToothService();

            var device = btPrint.GetDeviceList();

            if(device == null || device.Count == 0)
            {
                Toast.MakeText(ApplicationContext, "No printers found", ToastLength.Short).Show();
                return;
            }

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            OrdersViewModels._ordersWithToppings.ForEach(order =>
            {
                btPrint.Print(device[0], $"{order.OrderItemType.Name} {order.OrderItemType.BasePrice}");
                order.AvailableToppings.Where(t => t.Selected).ToList().ForEach(topping =>
                {
                    var charge = topping.Charge != 0 ? topping.Charge.ToString("C", nfi) : "";
                    btPrint.Print(device[0], $"{topping.Name} {charge}");
                });
            });


            var intent = new Intent(ApplicationContext, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            StartActivity(intent);
        }
    }
}