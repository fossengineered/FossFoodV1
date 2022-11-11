using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using FossFoodV1.OrderManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FossFoodV1.Sales
{
    public class SalesFragment : Fragment
    {
        View _v;
        List<string> _rows = new List<string> { "1" };
        ArrayAdapter<string> _itemsAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout._placeholder_sales, container, false);

            _v = v;

            Init(v);

            return v;
        }

        private void Init(View v)
        {    
           DisplaySales(DateTime.Now);
        }

        private void DisplaySales(DateTime serviceDate)
        {
            var orders = new OrderManagerEntity(serviceDate).GetOrders();

            _rows.Clear();

            var header = $"{"Date",-50}{"Order Id",-50}{"Customer Name",-50}{"Order Total",-50}";

            _rows.Add(header);

            foreach (var order in orders)
            {
                var baseAmount = order.OrderWithToppings.Sum(a => a.OrderItemType.BasePrice);
                order.OrderWithToppings.ForEach(a =>
                {
                    baseAmount += a.AvailableToppings.Where(b => b.Selected).Select(c => c.Charge).Sum();
                });

                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                var d = DateTime.ParseExact(order.ServiceDateId.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MMM, dd, yyyy");

                var row = $"|{d,-50}{order.OrdersId,-50}{order.CustomerName,-50}{baseAmount.ToString("C", nfi),-50}";

                _rows.Add(row);

            }

            _itemsAdapter = new ArrayAdapter<String>(Activity, Resource.Layout.sales_report_row, _rows);

            ListView listView = _v.FindViewById<ListView>(Resource.Id.lv_sales_report);
            listView.Adapter = _itemsAdapter;

            _itemsAdapter.NotifyDataSetChanged();

        }
    }
}