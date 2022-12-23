using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using FossFoodV1.Checklist;
using FossFoodV1.Sales;
using FossFoodV1.ServiceDates;
using FossFoodV1.Settings;
using FossFoodV1.Store;
using FossFoodV1.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FossFoodV1.MainWrapper
{
    static class Extras
    {
        public const string NAV_PAGE = "NAV_PAGE";

    }

    public enum NavPages
    {
        Service_Dates = 1,
        Tickets = 2,
        Checklist = 3,
        Sales = 4,
        Store = 5,
        Settings = 6
    }

    public static class NavPagesExtensions
    {
        public static string GetTitle(this NavPages page)
        {
            return page.ToString().Replace("_", " ");
        }

        public static AndroidX.Fragment.App.Fragment GetFragment(this NavPages page)
        {
            switch (page)
            {
                case NavPages.Service_Dates:
                    return new ServiceDatesFragment();
                case NavPages.Tickets:
                    return new TicketsFragment();
                case NavPages.Checklist:
                    return new ChecklistFragment();
                case NavPages.Sales:
                    return new SalesFragment();
                case NavPages.Store:
                    return new StoreFragment();
                case NavPages.Settings:
                    return new SettingsFragment();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}