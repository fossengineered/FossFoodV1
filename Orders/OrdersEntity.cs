using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FossFoodV1.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.Orders
{
    internal class OrdersEntity
    {
        FoodEntity _food = new FoodEntity();

        //public List<string> GetOrderItemTypes() => Enum.GetNames(typeof(OrderItemTypes)).OrderBy(a => a).ToList();
        //public List<string> GetOrderItemTypes() => _food.MenuItems.Food.OrderBy(a=>a.Name).Select(a=>a.Name).ToList();
    }
}