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

namespace FossFoodV1.ServiceDates
{
    [Table("ServiceDates")]
    public class ServiceDate
    {
        [PrimaryKey, AutoIncrement, Column("ServiceDateId")]
        public int ServiceDateId { get; set; }

        [Unique]
        public DateTime S_Date { get; set; }
    }
}