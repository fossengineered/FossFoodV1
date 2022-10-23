using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using FossFoodV1.Checklist;
using FossFoodV1.OrderManager;
using FossFoodV1.Sales;
using FossFoodV1.ServiceDates;
using FossFoodV1.Settings;
using FossFoodV1.Store;
using FossFoodV1.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.MainWrapper
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainWrapperActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_wrapper);

            ChangeScreen<ServiceDatesFragment>("Service Dates");

            FindViewById<View>(Resource.Id.nav_btn_schedule).Click += (a, b) =>
                ChangeScreen<ServiceDatesFragment>("Service Dates");

            FindViewById<View>(Resource.Id.nav_btn_tickets).Click += (a, b) =>
                ChangeScreen<TicketsFragment>("Tickets");

            FindViewById<View>(Resource.Id.nav_btn_checklist).Click += (a, b) =>
                ChangeScreen<ChecklistFragment>("Checklist");

            FindViewById<View>(Resource.Id.nav_btn_sales).Click += (a, b) =>
               ChangeScreen<SalesFragment>("Sales");

            FindViewById<View>(Resource.Id.nav_btn_store).Click += (a, b) =>
               ChangeScreen<StoreFragment>("Store");

            FindViewById<View>(Resource.Id.nav_btn_settings).Click += (a, b) =>
                ChangeScreen<SettingsFragment>("Settings");
        }

        private void ChangeScreen<T>(string title) where T : new()
        {
            SupportFragmentManager.BeginTransaction()
              .SetReorderingAllowed(true)
              .Replace(Resource.Id.main_content, new T() as AndroidX.Fragment.App.Fragment)
              .Commit();

            var tv = FindViewById<TextView>(Resource.Id.nav_header_title);
            tv.Typeface = Typeface.DefaultBold;
            tv.Text = title;
        }
    }
}