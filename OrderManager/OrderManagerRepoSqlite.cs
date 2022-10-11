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

        internal List<OrderManagerOrders> GetOpenOrders(DateTime serviceDate)
        {
            var m = _db.Table<OrderManagerOrders>().Where(x => x.RowStatus == RowStatus.Open);

            return m.ToList();
        }

        internal List<OrderManagerOrders> GetOpenClosed(DateTime serviceDate)
        {
            var m = _db.Table<OrderManagerOrders>().Where(x => x.RowStatus == RowStatus.Closed);

            return m.ToList();
        }

        void ValidateDb()
        {
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "foss_food.db3");

            //File.Delete(dbPath);

            _db = new SQLiteConnection(dbPath);

            _db.CreateTable<ServiceDate>();
            _db.CreateTable<OrderManagerOrders>();
        }

        internal void AddNewOrder(OrderManagerOrders order)
        {
            _db.Insert(order);
        }

        internal void CloseOrder(int orderId)
        {
            var t = _db.Get<OrderManagerOrders>(orderId);

            t.RowStatus = RowStatus.Closed;

            _db.Update(t);
        }

        internal void ReOpenOrder(int orderId)
        {
            var t = _db.Get<OrderManagerOrders>(orderId);

            t.RowStatus = RowStatus.Open;

            _db.Update(t);
        }
    }
}