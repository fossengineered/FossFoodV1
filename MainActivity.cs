using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using FossFoodV1.Orders;
using System;

namespace FossFoodV1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        OrdersViewModels _ordersVM;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.order_main);

            _ordersVM = new OrdersViewModels(this);
        }

        private void ContentMain_Click(object sender, EventArgs e)
        {
            var c = (Android.Widget.RelativeLayout)sender;

            //var v = LayoutInflater.Inflate(Resource.Layout.food_items, c);
            
            c.RemoveAllViews();
        }

        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

