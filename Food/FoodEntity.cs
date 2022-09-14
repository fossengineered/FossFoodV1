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
    internal class FoodEntity
    {
        public readonly MenuItems MenuItems;

        public FoodEntity()
        {
            var assembly = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(MenuItems)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("FossFoodV1.Resources.static_data.food-items.json");
            string text = "";

            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
                MenuItems = JsonConvert.DeserializeObject<MenuItems>(text);
            }
        }

        internal List<Topping> GetAvailableToppings(string foodItem)
        {
            var item = MenuItems.Items.First(a=>a.Name.Equals(foodItem, StringComparison.OrdinalIgnoreCase));
            
            var available = new List<Topping>();

            foreach(var t in item.Toppings)
            {
                var a = MenuItems.Toppings.First(a => a.Name.Equals(t, StringComparison.OrdinalIgnoreCase));

                a.Selected = item.AutoSelected != null && item.AutoSelected.Contains(t, StringComparer.OrdinalIgnoreCase);

                available.Add(a);
            }

            return available;
        }
    }
}