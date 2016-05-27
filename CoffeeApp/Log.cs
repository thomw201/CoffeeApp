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
    [Activity(Label = "Logs", Icon = "@drawable/applogo")]
    public class Log : Activity
    {
        private List<ReceivedLog> receivedLogs;
        private ListView lListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Log);
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

            lListView = FindViewById<ListView>(Resource.Id.logListView);
            receivedLogs = new List<ReceivedLog>();
            getLogs();
            LogAdapter profadpt = new LogAdapter(this, receivedLogs);
            lListView.Adapter = profadpt;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 0, 0, "Refresh");
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 0: 
                    this.Recreate(); //refresh page
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void getLogs()
        {
            //send controller request code for the names
            Communication.Send(Resources.GetString(Resource.String.requestlogscode));
            //wait to receive names
            string buffer = Communication.receiveMessage(Resources.GetString(Resource.String.requestlogscode));

            

            if (buffer != null) 
            {
                string[] dataArr = buffer.Split(',');
                for (int profileIndex = 0; profileIndex < dataArr.Length / 4; profileIndex++) // profile data = 6 entrys, div by 6 = amount of profiles
                {
                    try
                    {
                        receivedLogs.Add(new ReceivedLog(Int32.Parse(dataArr[0 + (4 * profileIndex)]), dataArr[1 + (4 * profileIndex)], Int32.Parse(dataArr[2 + (4 * profileIndex)]), dataArr[3 + (4 * profileIndex)]));
                    }
                    catch (Exception e)
                    {
                        Toast.MakeText(this, "Error parsing data..", ToastLength.Short).Show();
                    }
                }
            }
            else
                Toast.MakeText(this, "Connection timeout..", ToastLength.Short).Show();
        }

        private void actionbar_Click(object sender, EventArgs e)
        {
            LinearLayout buttonLayout = (LinearLayout)sender;
            switch (((TextView)buttonLayout.GetChildAt(1)).Text)
            {
                case "Coffee":
                    StartActivity(typeof(CoffeeOverview));
                    this.Finish();
                    break;
                case "Schedules":
                    StartActivity(typeof(Profiles));
                    this.Finish();
                    break;
                case "Logs":
                    break;
                case "Settings":
                    StartActivity(typeof(Settings));
                    this.Finish();
                    break;
            }
        }
    }
}