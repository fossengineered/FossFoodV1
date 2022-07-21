using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FossFoodV1.Food
{
    internal class FoodEntities
    {
        readonly Food[] FOODS;

        public FoodEntities()
        {
            var assembly = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(Foods)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("FossFoodV1.Resources.static_data.food-items.json");
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
                FOODS = JsonConvert.DeserializeObject<Foods>(text).Food;
            }
        }


    }
}