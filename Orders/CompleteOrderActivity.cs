﻿using Android.App;
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
            if (string.IsNullOrEmpty(_orderId))
                new OrderManagerEntity(new ServiceDatesEntity().Current)
                    .AddNewOrder(
                    OrdersViewModels._ordersWithToppings,
                    new OrderCustomerDetails
                    {
                        CustomerName = OrdersViewModels._customerName,
                        PagerNumber = OrdersViewModels._pagerNumber.Value
                    });
            else
            {
                new OrderManagerEntity(new ServiceDatesEntity().Current)
                .UpdateOrder(
                    int.Parse(_orderId),
                OrdersViewModels._ordersWithToppings,
                new OrderCustomerDetails
                {
                    CustomerName = OrdersViewModels._customerName,
                    PagerNumber = OrdersViewModels._pagerNumber.Value
                });
            }

            PrintTicket();

            var intent = new Intent(ApplicationContext, typeof(OrderChecklistActivity));
            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            StartActivity(intent);
        }

        private void PrintTicket()
        {
            try
            {
                var btPrint = new AndroidBlueToothService();

                var devices = btPrint.GetDeviceList();

                if (devices == null || devices.Count == 0)
                {
                    Toast.MakeText(ApplicationContext, "No printers found", ToastLength.Short).Show();
                    return;
                }

                foreach(var device in devices)
                {
                    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                    btPrint.Print(device, $"{DateTime.Now.ToShortTimeString()}");
                    btPrint.Print(device, "");

                    OrdersViewModels._ordersWithToppings.ForEach(order =>
                    {
                        btPrint.Print(device, "");

                        btPrint.Print(device, $"{order.OrderItemType.Name} -{order.OrderItemType.BasePrice}-");

                        order.AvailableToppings.Where(t => t.Selected).ToList().ForEach(topping =>
                        {
                            var charge = topping.Charge != 0 ? topping.Charge.ToString("C", nfi) : "";
                            btPrint.Print(device, $"   {topping.Name} -{charge}-");
                        });
                    });

                    btPrint.Print(device, "");
                    btPrint.Print(device, "");
                    btPrint.Print(device, "");
                }                
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
        }
    }
}