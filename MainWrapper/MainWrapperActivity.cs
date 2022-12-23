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

            var extra = Intent.GetIntExtra(Extras.NAV_PAGE, (int)NavPages.Service_Dates);

            ChangePage((NavPages)extra);

            FindViewById<View>(Resource.Id.nav_btn_schedule).Click += (a, b) => ChangePage(NavPages.Service_Dates);

            FindViewById<View>(Resource.Id.nav_btn_tickets).Click += (a, b) => ChangePage(NavPages.Tickets);

            FindViewById<View>(Resource.Id.nav_btn_checklist).Click += (a, b) => ChangePage(NavPages.Checklist);

            FindViewById<View>(Resource.Id.nav_btn_sales).Click += (a, b) => ChangePage(NavPages.Sales);

            FindViewById<View>(Resource.Id.nav_btn_store).Click += (a, b) => ChangePage(NavPages.Store);

            FindViewById<View>(Resource.Id.nav_btn_settings).Click += (a, b) => ChangePage(NavPages.Settings);
        }

        private void ChangePage(NavPages page)
        {
            SupportFragmentManager.BeginTransaction()
              .SetReorderingAllowed(true)
              .Replace(Resource.Id.main_content, page.GetFragment())
              .Commit();

            var tv = FindViewById<TextView>(Resource.Id.nav_header_title);
            tv.Typeface = Typeface.DefaultBold;
            tv.Text = page.GetTitle();
        }
    }
}