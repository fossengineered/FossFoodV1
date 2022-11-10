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

namespace FossFoodV1.Checklist
{
    internal class ChecklistEntity
    {
        internal void AddChecklist(string cName) =>
            new ChecklistRepoSqlite().AddChecklist(cName);

        internal List<Checklist> GetChecklists() =>
                new ChecklistRepoSqlite().GetChecklists();
    }
}