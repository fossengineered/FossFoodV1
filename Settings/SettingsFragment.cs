using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.Fragment.App;
using FossFoodV1.OrderManager;
using FossFoodV1.ServiceDates;
using Google.Android.Material.FloatingActionButton;
using Java.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FossFoodV1.Settings
{
    public class SettingsFragment : Fragment
    {
        View _v;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        private void Init(View v)
        {
            var orderManager = new OrderManagerEntity(new ServiceDatesEntity().Current);

            v.FindViewById<View>(Resource.Id.cv_setting_backup).Click += (a, b) =>
            {
                string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "foss_food.db3");


                File file = new File(dbPath);

                var l = file.Exists();

                // wrap File object into a content provider. NOTE: authority here should match authority in manifest declaration
                var fileUri = FileProvider.GetUriForFile(Activity, "com.companyname.fossfoodv1.fileprovider", file);

                Intent shareIntent = new Intent(Intent.ActionSend);
                shareIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                shareIntent.PutExtra(Intent.ExtraStream, fileUri);
                shareIntent.SetDataAndType(fileUri, "application/vnd.sqlite3");

                StartActivity(Intent.CreateChooser(shareIntent, "Save DB using"));
            };
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout._placeholder_settings, container, false);

            _v = v;

            Init(v);

            return v;
        }
    }
}