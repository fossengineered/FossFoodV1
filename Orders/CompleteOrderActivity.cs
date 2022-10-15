using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FossFoodV1.OrderManager;
using FossFoodV1.ServiceDates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using XFBluetoothPrint.Droid;

namespace FossFoodV1.Orders
{
    [Activity(Label = "CompleteOrderActivity", Theme = "@style/AppTheme.NoActionBar")]
    public class CompleteOrderActivity : Activity
    {
        string _orderId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.order_complete_order);

            _orderId = Intent.GetStringExtra("order_id");

            FindViewById<Button>(Resource.Id.btn_complete_order).Click += CompleteOrderActivity_Click;
            //FindViewById<Button>(Resource.Id.btn_print_order).Click += PrintOrder_Click; 

            SetOrderTotal();
        }

        private void PrintOrder_Click(object sender, EventArgs e)
        {
            //var btPrint = new AndroidBlueToothService();

            //var device = btPrint.GetDeviceList();

            //if (device == null || device.Count == 0)
            //{
            //    Toast.MakeText(ApplicationContext, "No printers found", ToastLength.Short).Show();
            //    return;
            //}

            //NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            //OrdersViewModels._ordersWithToppings.ForEach(order =>
            //{
            //    btPrint.Print(device[0], $"{order.OrderItemType.Name} {order.OrderItemType.BasePrice}");
            //    order.AvailableToppings.Where(t => t.Selected).ToList().ForEach(topping =>
            //    {
            //        var charge = topping.Charge != 0 ? topping.Charge.ToString("C", nfi) : "";
            //        btPrint.Print(device[0], $"{topping.Name} {charge}");
            //    });
            //});
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
            OrderManagerOrders order;

            if (string.IsNullOrEmpty(_orderId))
                order = new OrderManagerEntity(new ServiceDatesEntity().Current)
                    .AddNewOrder(
                    OrdersViewModels._ordersWithToppings,
                    new OrderCustomerDetails
                    {
                        CustomerName = OrdersViewModels._customerName,
                        PagerNumber = OrdersViewModels._pagerNumber.Value
                    });
            else
            {
                order=new OrderManagerEntity(new ServiceDatesEntity().Current)
                .UpdateOrder(
                    int.Parse(_orderId),
                OrdersViewModels._ordersWithToppings,
                new OrderCustomerDetails
                {
                    CustomerName = OrdersViewModels._customerName,
                    PagerNumber = OrdersViewModels._pagerNumber.Value
                });
            }

            PrintTicket(order);

            var intent = new Intent(ApplicationContext, typeof(OrderChecklistActivity));
            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            StartActivity(intent);
        }

        private void PrintTicket(OrderManagerOrders order)
        {
            try
            {
                
                var btPrint = new AndroidBlueToothService();

                //if (devices == null || devices.Count == 0)
                //{
                //    Toast.MakeText(ApplicationContext, "No printers found", ToastLength.Short).Show();
                //    return;
                //}

                List<string> lines = new List<string>(50);

                lines.Add("");
                lines.Add("");
                lines.Add("");

                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                lines.Add($"Order # {order.OrdersId}");
                lines.Add($"{DateTime.Now.ToShortTimeString()}");
                lines.Add("");
                lines.Add($"Customer {order.CustomerName}");
                lines.Add($"Pager #{order.PagerNumber}");
                lines.Add("");

                OrdersViewModels._ordersWithToppings.ForEach(order =>
                {
                    lines.Add("");

                    lines.Add($"{order.OrderItemType.Name}");

                    order.AvailableToppings.Where(t => t.Selected).ToList().ForEach(topping =>
                    {
                        var charge = topping.Charge != 0 ? topping.Charge.ToString("C", nfi) : "";

                        var tab = topping.SpecialCallout ? "***" : "   ";
                        lines.Add($"{tab}{topping.Name}");
                    });
                });

                if (lines.Count <=6)
                {
                    for (var x = 0; x < 6 - lines.Count; x++)
                    {
                        lines.Add("");
                    }
                }

                lines.Add("============END============");

                btPrint.BulkPrint(lines, message=> Toast.MakeText(ApplicationContext, message, ToastLength.Short).Show());
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
        }
    }
}