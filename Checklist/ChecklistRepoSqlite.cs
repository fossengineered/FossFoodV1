using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FossFoodV1.OrderManager;
using FossFoodV1.ServiceDates;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.Checklist
{
    internal class ChecklistRepoSqlite
    {
        SQLiteConnection _db;

        public ChecklistRepoSqlite()
        {
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "foss_food.db3");

            //File.Delete(dbPath);

            _db = new SQLiteConnection(dbPath);

            _db.CreateTable<Checklist>();
        }

        internal void AddChecklist(string cName)
        {
            _db.Insert(new Checklist { Name = cName });
        }

        internal List<Checklist> GetChecklists()
        {
            return _db.Table<Checklist>().ToList();
        }

    }
}