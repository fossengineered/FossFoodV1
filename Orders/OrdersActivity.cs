using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.App;
using FossFoodV1.OrderManager;
using FossFoodV1.Orders;
using FossFoodV1.ServiceDates;
using System;
using XFBluetoothPrint.Droid;

namespace FossFoodV1.Orders
{
    [Activity(Label = "OrdersActivity", Theme = "@style/AppTheme.NoActionBar")]
    public class OrdersActivity : Activity
    {
        OrdersViewModels _ordersVM;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.order_main);

            _ordersVM = new OrdersViewModels(this);
            _ordersVM.ClearData();

            var orderId = Intent.GetStringExtra("order_id");

            if (string.IsNullOrEmpty(orderId)) return;            

            _ordersVM.PopulateOrder(int.Parse(orderId));
        }

        protected void SetupParent(View view)
        {
            //Set up touch listener for non-text box views to hide keyboard.
            if (view.GetType() != typeof(EditText))
            {
                view.Touch += (sender, e) =>
                {
                    var inputManager = (InputMethodManager)GetSystemService(InputMethodService);
                    inputManager.HideSoftInputFromWindow(CurrentFocus?.WindowToken, HideSoftInputFlags.None);
                };
            }

            //If a layout container, iterate over children
            if (view.GetType() != typeof(ViewGroup))
            {
                var v = (ViewGroup)view;
                for (int i = 0; i < v.ChildCount; i++)
                {
                    View innerView = ((ViewGroup)view).GetChildAt(i);
                    SetupParent(innerView);
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent? ev)
        {
            var inputManager = (InputMethodManager)GetSystemService(InputMethodService);
            inputManager.HideSoftInputFromWindow(CurrentFocus?.WindowToken, HideSoftInputFlags.None);

            return base.OnTouchEvent(ev);

            //if (ev.Action != MotionEventActions.Down)
            //    return base.OnTouchEvent(ev);

            //if (!(CurrentFocus is EditText))
            //    return base.OnTouchEvent(ev);

            //Rect outRect = new Rect();
            //CurrentFocus.GetGlobalVisibleRect(outRect);

            //if (outRect.Contains((int)ev.RawX, (int)ev.RawY))
            //    return base.OnTouchEvent(ev);

            //CurrentFocus.ClearFocus();

            ////InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            ////imm.HideSoftInputFromWindow(CurrentFocus?.WindowToken, 0);

            //return base.OnTouchEvent(ev);
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