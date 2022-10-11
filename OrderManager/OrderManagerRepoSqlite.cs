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
using System.IO;
using System.Linq;
using System.Text;

namespace FossFoodV1.OrderManager
{
    internal class OrderManagerRepoSqlite
    {
        SQLiteConnection _db;

        public OrderManagerRepoSqlite(DateTime serviceDate)
        {
            ValidateDb();
        }

        void ValidateDb()
        {
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "foss_food.db3");

            File.Delete(dbPath);

            _db = new SQLiteConnection(dbPath);

            _db.CreateTable<ServiceDate>();
            _db.CreateTable<OrderManagerOrders>();

        }

        internal void AddNewOrder(OrderManagerOrders order)
        {
            _db.Insert(order);

            var t = _db.Table<OrderManagerOrders>().ToList();
        }
    }
}