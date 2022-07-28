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
    internal class OrdersEntity
    {
        public List<string> GetOrderItemTypes() => Enum.GetNames(typeof(OrderItemTypes)).OrderBy(a => a).ToList();


    }
}