using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FossFoodV1.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.OrderManager
{
    internal class OrderManagerEntity
    {
        static List<OrderWithToppings> _openOrders;
        static List<OrderWithToppings> _closedOrders;

        public List<OrderWithToppings> OpenOrders { get { return _openOrders; } }
        public List<OrderWithToppings> CloseOrders { get { return _closedOrders; } }

        public OrderManagerEntity(DateTime serviceDate)
        {
            new OrderManagerRepoSqlite().ValidateDb(serviceDate);
        }

        internal void Init()
        {
            _openOrders = new List<OrderWithToppings>();
            _closedOrders = new List<OrderWithToppings>();
        }
    }
}