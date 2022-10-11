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
    public class OrderWithToppings
    {
        public FoodItem OrderItemType { get; set; }
        public List<Topping> AvailableToppings { get; set; }
        
    }

    public class OrderCustomerDetails
    {
        public string CustomerName { get; set; }
        public int PagerNumber { get; set; }
    }

    //public enum OrderItemTypes
    //{
    //   Burger_Thornton,
    //   Burger_Single,
    //   Funnel_Cake_Classic,
    //   Fried_Oreos,
    //   Hotdog,
    //   Fries
    //}

    //public enum OrderToppingTypes
    //{
    //    Cheese_Nacho,
    //    Cheese_American_Yellow,
    //    Cheese_American_White,
    //    Chili,
    //    Lettuce,
    //    Tomato,
    //    Pickles

    //}
}