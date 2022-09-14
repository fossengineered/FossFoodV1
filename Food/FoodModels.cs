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

namespace FossFoodV1.Food
{

    public class MenuItems
    {
        public FoodItem[] Items { get; set; }
        public Topping[] Toppings { get; set; }
    }

    public class FoodItem
    {
        public string Name { get; set; }
        public string[] Toppings { get; set; }
        public float BasePrice { get; set; }
        public string[] AutoSelected { get; set; }
    }

    public class Topping
    {
        public string Name { get; set; }
        public float Charge { get; set; }
        public bool Selected { get; set; }
    }

}