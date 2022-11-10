using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using FossFoodV1.OrderManager;
using Google.Android.Material.Tabs;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace FossFoodV1.Checklist
{
    public class ChecklistFragment : Fragment
    {
        bool _isBoxVisible;
        TabLayout _tabLayout;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var v = inflater.Inflate(Resource.Layout._placeholder_checklist, container, false);

            v.FindViewById<TextView>(Resource.Id.btn_accept_add_checklist).Click += BtnAcceptAddChecklist_Click;

            _tabLayout = v.FindViewById<TabLayout>(Resource.Id.checklist_tabs); // get the reference of TabLayout
            _tabLayout.AddOnTabSelectedListener(new OnTabSelectedListener(v, (a) => _isBoxVisible = a, () => _isBoxVisible));

            TabLayout.Tab addTab = _tabLayout.NewTab(); // Create a new Tab names
            addTab.SetText("+Add Tab"); // set the Text for the first Tab
            addTab.SetIcon(Resource.Drawable.abc_ic_menu_copy_mtrl_am_alpha); // set an icon for the first tab

            var checkLists = new ChecklistEntity().GetChecklists();

            _tabLayout.AddTab(addTab);

            foreach (var list in checkLists)
            {
                TabLayout.Tab tab = _tabLayout.NewTab(); // Create a new Tab names
                tab.SetText(list.Name); // set the Text for the first Tab
                tab.SetIcon(Resource.Drawable.abc_ic_menu_copy_mtrl_am_alpha); // set an icon for the first tab

                _tabLayout.AddTab(tab);
            }
            InitRecycler(v);
            return v;
        }

        private void InitRecycler(View v)
        {
            var orderItemRecycler = v.FindViewById<RecyclerView>(Resource.Id.checklist_recycler);

            var layoutManager = new LinearLayoutManager(v.Context) { Orientation = LinearLayoutManager.Vertical };
            orderItemRecycler.SetLayoutManager(layoutManager);
            orderItemRecycler.HasFixedSize = true;

            var adapter = new ChecklistRecycler(Activity, new List<ChecklistItem> { new ChecklistItem() }, (a) => { } );

            orderItemRecycler.SetAdapter(adapter);
        }

        private void BtnAcceptAddChecklist_Click(object sender, EventArgs e)
        {
            View.FindViewById<View>(Resource.Id.enter_checklist_name_box).Visibility = ViewStates.Invisible;
            _isBoxVisible = false;

            var cName = View.FindViewById<EditText>(Resource.Id.txt_checklist_name).Text;

            TabLayout.Tab tab = _tabLayout.NewTab(); // Create a new Tab names
            tab.SetText(cName); // set the Text for the first Tab
            tab.SetIcon(Resource.Drawable.abc_ic_menu_copy_mtrl_am_alpha); // set an icon for the first tab

            _tabLayout.AddTab(tab);

            new ChecklistEntity().AddChecklist(cName);
        }

        private class OnTabSelectedListener : Java.Lang.Object, TabLayout.IOnTabSelectedListener
        {
            View _v;
            bool _isInitialLoad = true;
            Action<bool> _setVisibility;
            Func<bool> _getVisibility;

            public OnTabSelectedListener(View v, Action<bool> setVisibility, Func<bool> getVisibility)
            {
                _v = v;
                _setVisibility = setVisibility;
                _getVisibility = getVisibility;
            }

            public void OnTabReselected(TabLayout.Tab tab)
            {
                TabSelected(tab);
            }

            public void OnTabSelected(TabLayout.Tab tab)
            {
                if (_isInitialLoad)
                {
                    _isInitialLoad = false;
                    return;
                }

                TabSelected(tab);
            }

            private void TabSelected(TabLayout.Tab tab)
            {
                switch (tab.Position)
                {
                    case 0:
                        _v.FindViewById<View>(Resource.Id.enter_checklist_name_box).Visibility 
                            = _getVisibility() ? ViewStates.Invisible : ViewStates.Visible;
                        _setVisibility(!_getVisibility());
                        break;
                    default:
                        return;
                }
            }

            public void OnTabUnselected(TabLayout.Tab tab)
            {
            }
        }
    }
}