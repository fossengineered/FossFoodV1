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

            SetOrderTotal();
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
            var intent = new Intent(ApplicationContext, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            StartActivity(intent);
        }
    }
}