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
    public class Foods
    {
        public Food[] Food { get; set; }
    }

    public class Food
    {
        public string Name { get; set; }
    }
}