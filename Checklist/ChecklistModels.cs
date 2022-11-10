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

namespace FossFoodV1.Checklist
{
    [Table("Checklists")]
    public class Checklist
    {
        [PrimaryKey, AutoIncrement]
        public int ChecklistId { get; set; }
        [NotNull]
        public string Name { get; set; }

    }

    [Table("ChecklistItems")]
    public class ChecklistItem
    {
        [PrimaryKey, AutoIncrement]
        public int ChecklistItemId { get; set; }
        [NotNull]
        public int ChecklistId { get; set; }
        [NotNull]
        public string ItemText { get; set; }
    }
}