using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
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
        }

        private void CompleteOrderActivity_Click(object sender, EventArgs e)
        {
            var intent = new Intent(ApplicationContext, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            StartActivity(intent);
        }
    }
}