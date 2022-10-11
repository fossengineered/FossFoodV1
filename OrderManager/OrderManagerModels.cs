using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.OrderManager
{
    public class OrderManagerOrders
    {
        [PrimaryKey, AutoIncrement]
        public int OrdersId { get; set; }
        [NotNull]
        public int ServiceDateId { get; set; }
        [NotNull]
        public string CustomerName { get; set; }
        [NotNull]
        public int PagerNumber { get; set; }
        [NotNull]
        public string OrderData { get; set; }
        [NotNull]        
        public RowStatus RowStatus { get; set; }
        [NotNull]
        public DateTime CreatedOn { get; set; }
        
    }
}