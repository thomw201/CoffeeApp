using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CoffeeApp
{
    [Activity(Label = "Profiles", Icon = "@drawable/applogo")] //, Theme="@style/CustomActionBarTheme"
    public class Profiles : Activity
    {
        //private List<string> profileNames;
        private List<Profile> retrievedProfiles;
        private ListView pListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Profiles);
            //load action bar
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);
            ActionBar.SetDisplayShowCustomEnabled(true);

            ActionBar.SetCustomView(Resource.Layout.action_bar);

            //add listeners to the actionbar
            LinearLayout action1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            LinearLayout action2 = FindViewById<LinearLayout>(Resource.Id.linearLayout2);
            LinearLayout action3 = FindViewById<LinearLayout>(Resource.Id.linearLayout3);
            LinearLayout action4 = FindViewById<LinearLayout>(Resource.Id.linearLayout4);
            action1.Click += actionbar_Click;
            action2.Click += actionbar_Click;
            action3.Click += actionbar_Click;
            action4.Click += actionbar_Click;

            pListView = FindViewById<ListView>(Resource.Id.profilesListView);
            retrievedProfiles = new List<Profile>();
            //pListView.SetBackgroundColor(Android.Graphics.Color.Rgb(42, 42, 42));
            getProfiles();
            ProfileAdapter profadpt = new ProfileAdapter(this, retrievedProfiles);
            pListView.Adapter = profadpt;
            pListView.ItemClick += PListView_ItemClick;
            pListView.ItemLongClick += PListView_ItemLongClick;
            //Toast.MakeText(this, "Loaded " + retrievedProfiles.Count() + " profiles!", ToastLength.Short).Show();
        }

        private void PListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {

            Android.App.AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alertDialog = builder.Create();
            alertDialog.SetTitle(retrievedProfiles[e.Position].getName());
            alertDialog.SetIcon(Resource.Drawable.applogo);
            alertDialog.SetMessage("Delete this profile?");
            alertDialog.SetButton("Delete", (s, ev) =>
            {
                Communication.Send(Resources.GetString(Resource.String.deleteprofilecode) + retrievedProfiles[e.Position].getID());
                this.Recreate(); //refresh
                Toast.MakeText(this, "Profile " + retrievedProfiles[e.Position].getName() + " deleted.", ToastLength.Short).Show();
            });
            alertDialog.SetButton2("Cancel", (s, ev) =>
            {
                //close?
            });
            alertDialog.Show();


        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 0, 0, "New Profile");
            menu.Add(0, 1, 1, "Refresh");
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 0: //new profile
                    StartActivity(typeof(NewProfile));
                    this.Finish();
                    return true;
                case 1:
                    this.Recreate(); //refresh page
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        private void PListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(EditProfile));
            intent.PutExtra("profileID", retrievedProfiles[e.Position].getID());
            intent.PutExtra("profileName", retrievedProfiles[e.Position].getName());
            intent.PutExtra("profileTime", retrievedProfiles[e.Position].getStartTime());
            intent.PutExtra("profileStayontime", retrievedProfiles[e.Position].getStayontime());
            intent.PutExtra("profileActive", retrievedProfiles[e.Position].getActive());
            intent.PutExtra("profileDays", retrievedProfiles[e.Position].getDays());
            StartActivity(intent);
            this.Finish();
        }

        private void getProfiles()
        {
            //send controller request code for the names
            Communication.Send(Resources.GetString(Resource.String.requestprofilescode));
            //wait to receive names
            string buffer = Communication.receiveMessage(Resources.GetString(Resource.String.requestprofilescode));

           
            if (buffer != null)
            {
                string[] dataArr = buffer.Split(',');
                for (int profileIndex = 0; profileIndex < dataArr.Length / 6; profileIndex++) // profile data = 6 entrys, div by 6 = amount of profiles
                {
                    try
                    {
                        retrievedProfiles.Add(new Profile(Int32.Parse(dataArr[0 + (6 * profileIndex)]), dataArr[1 + (6 * profileIndex)], Int32.Parse(dataArr[2 + (6 * profileIndex)]), Int32.Parse(dataArr[3 + (6 * profileIndex)]), Int32.Parse(dataArr[4 + (6 * profileIndex)]), (dataArr[5 + (6 * profileIndex)])));
                    }
                    catch (Exception e)
                    {
                        Toast.MakeText(this, "Error parsing data..", ToastLength.Short).Show();
                    }
                }
            }
           // else//incorrect data received, retry?
           //     Toast.MakeText(this, "Incorrect data received..", ToastLength.Short).Show();
        }

        private void actionbar_Click(object sender, EventArgs e)
        {
            LinearLayout buttonLayout = (LinearLayout)sender;
            switch (((TextView)buttonLayout.GetChildAt(1)).Text)
            {
                case "Coffee":
                    StartActivity(typeof(CoffeeOverview));
                    break;
                case "Schedules":
                    break;
                case "Logs":
                    StartActivity(typeof(Log));
                    break;
                case "Settings":
                    StartActivity(typeof(Settings));
                    this.Finish();
                    break;
            }
        }
    }
}