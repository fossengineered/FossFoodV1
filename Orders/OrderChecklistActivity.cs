using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FossFoodV1.OrderManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.Orders
{
    [Activity(Label = "OrderChecklistActivity", Theme = "@style/AppTheme.NoActionBar")]
    public class OrderChecklistActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.order_completed_checklist);

            FindViewById<Button>(Resource.Id.order_confirm_checklist).Click += OrderChecklistActivity_Click;
        }

        private void OrderChecklistActivity_Click(object sender, EventArgs e)
        {
            var intent = new Intent(ApplicationContext, typeof(OrderManagerActivity));
            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            StartActivity(intent);
        }
    }
}