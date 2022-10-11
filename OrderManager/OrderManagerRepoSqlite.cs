using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FossFoodV1.ServiceDates;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.OrderManager
{
    internal class OrderManagerRepoSqlite
    {
        public void ValidateDb(DateTime serviceDate)
        {
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "foss_food.db3");

            var db = new SQLiteConnection(dbPath);

            db.CreateTable<ServiceDate>();

            var table = db.Table<ServiceDate>();

            if (!table.Any())
            {
                db.Insert(new ServiceDate
                {
                    S_Date =serviceDate 
                });
            }

            var sDate = table.Last();
        }
    }
}