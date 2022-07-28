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
    public class OrderWithToppings
    {
       public OrderItemTypes OrderItemType { get; set; }

    }

    public enum OrderItemTypes
    {
       Burger_Thornton,
       Burger_Single,
       Funnel_Cake_Classic,
       Fried_Oreos,
       Hotdog,
       Fries
    }
}