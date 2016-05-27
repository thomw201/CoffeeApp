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
using System.Threading;
using System.Threading.Tasks;
using Android.Graphics;

namespace CoffeeApp
{
    [Activity(Label = "CoffeeOverview", Icon = "@drawable/applogo")]
    public class CoffeeOverview : Activity
    {
        TextView loadedProfilesTxtView;
        TextView SystimeTxtview;
        TextView tempTxtview;
        TextView statusTxtView;
        Thread thread;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CoffeeOverview);
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

            loadedProfilesTxtView = FindViewById<TextView>(Resource.Id.loadedProfilesTxt);
            SystimeTxtview = FindViewById<TextView>(Resource.Id.sysTimeTxtView);
            tempTxtview = FindViewById<TextView>(Resource.Id.tempTxtView);
            statusTxtView = FindViewById<TextView>(Resource.Id.statusTxtView);
            setProfileNames();
            thread = new Thread(new ThreadStart(updateOverviewThread));
            thread.Start();

        }

        public void updateOverviewThread()
        {
            //string overviewMsg = "2402"; // request system time, status & temperature
            while (true)
            {
                Thread.Sleep(Timers.overViewUpdateInterval);
                Communication.Send(Resources.GetString(Resource.String.requestoverviewcode)); // request todays profiles
                //wait to receive
                string buffer = Communication.receiveMessage(Resources.GetString(Resource.String.requestoverviewcode));
                string datetime = "";
                string temperature = "";
                string status = "";


                if (buffer != null)
                {
                    string[] dataArr = buffer.Split(',');
                    //date & time
                    datetime = dataArr[0];
                    //Status
                    switch (int.Parse(dataArr[1]))
                    {
                        case 0:
                            //statusTxtView.SetTextColor(Color.Chocolate);
                            status = "Idle";
                            break;
                        case 1:
                            //loadedProfilesTxtView.SetTextColor(Color.Aqua);
                            status = "Busy";
                            break;
                        case 2:
                            //loadedProfilesTxtView.SetTextColor(Color.Green);
                            status = "Done";
                            break;
                    }
                    temperature = dataArr[2];
                }
                else
                    Toast.MakeText(this, "Connection timeout..", ToastLength.Short).Show();
                RunOnUiThread(() =>
                {
                    SystimeTxtview.Text = "System time: " + datetime;
                    tempTxtview.Text = temperature + "°c";
                    statusTxtView.Text = "Status: " + status;
                });
            }
        }

        private void setProfileNames()
        {
            Communication.Send(Resources.GetString(Resource.String.requesttodaysprofiles)); 
            //wait to receive
            string buffer = Communication.receiveMessage(Resources.GetString(Resource.String.requesttodaysprofiles));
            int profiles = 0;


            if (buffer != null)
            {
                string[] dataArr = buffer.Split(',');

                for (int profileIndex = 0; profileIndex < dataArr.Length / 2; profileIndex++)
                {
                    try
                    {
                        if (profiles > 0) // if there's already a profile, add next onto it
                        {
                            loadedProfilesTxtView.Text += "\nProfile: " + dataArr[0 + (2 * profileIndex)];
                        }
                        else
                            loadedProfilesTxtView.Text = "\nProfile: " + dataArr[0 + (2 * profileIndex)];
                        profiles++;
                        loadedProfilesTxtView.SetTextColor(Color.Aqua);
                        loadedProfilesTxtView.Text += " at " + Timers.getTimeFormat(Convert.ToInt32(dataArr[1 + (2 * profileIndex)]), true);
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
                    break;
                case "Schedules":
                    thread.Abort();
                    StartActivity(typeof(Profiles));
                    this.Finish();
                    break;
                case "Logs":
                    thread.Abort();
                    StartActivity(typeof(Log));
                    this.Finish();
                    break;
                case "Settings":
                    thread.Abort();
                    StartActivity(typeof(Settings));
                    this.Finish();
                    break;
            }
        }
    }
}