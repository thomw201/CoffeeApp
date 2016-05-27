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
    [Activity(Label = "Settings", Icon = "@drawable/applogo")]
    public class Settings : Activity
    {
        private ListView sListView;
        bool sound = false;
        int timeThreadInter;
        int mainThreadInter;
        EditText overviewInterval;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings);
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
            action1.ClearFocus();
            action1.Click += actionbar_Click;
            action2.Click += actionbar_Click;
            action3.Click += actionbar_Click;
            action4.Click += actionbar_Click;

            getSettings(); //get current settings from raspi controller

            ToggleButton soundBtn = FindViewById<ToggleButton>(Resource.Id.soundToggleButton);
            soundBtn.Checked = sound;
            EditText timeInterval = FindViewById<EditText>(Resource.Id.timeUpdateIntervalTxt);
            timeInterval.Text = Convert.ToString(timeThreadInter);
            EditText mainInterval = FindViewById<EditText>(Resource.Id.mainUpdateIntervalTxt);
            mainInterval.Text = Convert.ToString(mainThreadInter);
            overviewInterval = FindViewById<EditText>(Resource.Id.OverviewUpdateTxt);
            overviewInterval.Text = Convert.ToString(Timers.overViewUpdateInterval);
            Button Rbutton = FindViewById<Button>(Resource.Id.Rbutton);
            Button Bbutton = FindViewById<Button>(Resource.Id.Bbutton);
            Button Gbutton = FindViewById<Button>(Resource.Id.Gbutton);
            Button OFFbutton = FindViewById<Button>(Resource.Id.OFFbutton);
            Button saveButton = FindViewById<Button>(Resource.Id.saveButton);
            Rbutton.Click += button_Click;
            Bbutton.Click += button_Click;
            Gbutton.Click += button_Click;
            OFFbutton.Click += OFFbutton_Click;
            saveButton.Click += saveButton_Click;

            soundBtn.Click += SoundBtn_Click;

            sListView = FindViewById<ListView>(Resource.Id.profilesListView);
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            int overviewTime;

            if (int.TryParse(overviewInterval.Text, out overviewTime))
            {
                if (overviewTime > 250)
                {
                    Timers.overViewUpdateInterval = overviewTime;
                }
                else
                    Toast.MakeText(this, "Overview interval too low.", ToastLength.Short).Show();
            }
            else
                Toast.MakeText(this, "Invalid overview time.", ToastLength.Short).Show();
        }
        //switch off all leds
        private void OFFbutton_Click(object sender, EventArgs e)
        {
            Communication.Send(Resources.GetString(Resource.String.ledsoffcode));
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            Communication.Send(Resources.GetString(Resource.String.setcolorcode) + b.Text);
        }

        private void SoundBtn_Click(object sender, EventArgs e)
        {
            sound = !sound;
            Communication.Send(Resources.GetString(Resource.String.switchoundcode) + sound);
        }

        private void getSettings()
        {
            //send controller request for the settings
            Communication.Send(Resources.GetString(Resource.String.requestsettings));
            //wait to receive
            string buffer = Communication.receiveMessage(Resources.GetString(Resource.String.requestsettings));

            
            if (buffer != null)
            {
                string[] dataArr = buffer.Split(',');
                for (int settingsIndex = 0; settingsIndex < dataArr.Length / 3; settingsIndex++)
                {
                    try
                    {
                        sound = bool.Parse(dataArr[0]);

                        timeThreadInter = int.Parse(dataArr[1]);

                        mainThreadInter = int.Parse(dataArr[2]);
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
                    StartActivity(typeof(Log));
                    this.Finish();
                    break;
                case "Settings":
                    break;
            }
        }
    }
}