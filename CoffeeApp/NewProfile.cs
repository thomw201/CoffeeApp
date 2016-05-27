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
using Android.Graphics;

namespace CoffeeApp
{
    [Activity(Label = "New Profile", Icon = "@drawable/applogo")]
    public class NewProfile : Activity
    {
        List<int> dayList;
        EditText profName;
        TextView profTime;
        TextView profStayon;
        CheckBox activeChkbx;
        int stayontime = 0;
        int stayOnTimeCounter = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //use the content view of edit profile (it's the same style)
            SetContentView(Resource.Layout.EditProfile);
            //save and cancel button trigger events
            Button SaveButton = FindViewById<Button>(Resource.Id.saveButton);
            SaveButton.Click += saveButtonClick;
            Button CancelButton = FindViewById<Button>(Resource.Id.cancelButton);
            CancelButton.Click += cancelButtonClick;
            //Title textview
            TextView Title = FindViewById<TextView>(Resource.Id.EditProfileTitle);
            Title.Text = "New Profile";
            //profile name TextEdit
            profName = FindViewById<EditText>(Resource.Id.profilenameEditText);
            profTime = FindViewById<TextView>(Resource.Id.timeLabel);
            profTime.Text = "12:00";
            profStayon = FindViewById<TextView>(Resource.Id.StayontimeTxtLabel);
            activeChkbx = FindViewById<CheckBox>(Resource.Id.ActiveCheckbox);
            //init daybuttons
            List<TextView> dayButtons = new List<TextView>();
            dayButtons.Add(FindViewById<TextView>(Resource.Id.mondayTextView));
            dayButtons.Add(FindViewById<TextView>(Resource.Id.tuesdayTextView));
            dayButtons.Add(FindViewById<TextView>(Resource.Id.wednesdayTextView));
            dayButtons.Add(FindViewById<TextView>(Resource.Id.thursdayTextView));
            dayButtons.Add(FindViewById<TextView>(Resource.Id.fridayTextView));
            dayButtons.Add(FindViewById<TextView>(Resource.Id.saturdayTextView));
            dayButtons.Add(FindViewById<TextView>(Resource.Id.sundayTextView));
            dayButtons.Add(FindViewById<TextView>(Resource.Id.everydayTextView));
            //attach clickevent to each day button
            foreach (TextView daybutton in dayButtons)
            {
                daybutton.Click += dayClicked;
            }
            dayList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
            profTime.TextSize = 20;
            profTime.SetTextColor(Color.Aqua);
            profTime.Click += delegate
            {
                TimePickerFragment pickTime = new TimePickerFragment(profTime);
                pickTime.Show(this.FragmentManager, "Starttime");
            };
            profStayon.Click += delegate
            {
                if (stayOnTimeCounter == Timers.StayOnTimes.Length)
                {
                    stayOnTimeCounter = 0;
                }
                stayontime = Timers.StayOnTimes[stayOnTimeCounter];
                profStayon.Text = Timers.getFriendlyMinutes(stayontime);
                stayOnTimeCounter++;
            };
        }
        private void cancelButtonClick(object sender, EventArgs e)
        {
            StartActivity(typeof(Profiles));
            this.Finish();
        }

        private void saveButtonClick(object sender, EventArgs e)
        {
            int timeResult;
            if (int.TryParse((profTime.Text).Replace(":", ""), out timeResult))
            {
                string profileData = String.Format("{0},{1},{2},{3},", profName.Text, timeResult, stayontime, Convert.ToInt32(activeChkbx.Checked));
                foreach (int day in dayList)
                {
                    profileData += day.ToString();
                }
                Communication.Send(Resources.GetString(Resource.String.newprofilecode) + profileData);
                StartActivity(typeof(Profiles));
                this.Finish();
            }
            else
                Toast.MakeText(this, "Enter all fields before saving.", ToastLength.Short).Show();
        }

        private void dayClicked(object sender, EventArgs e)
        {
            TextView txt = sender as TextView;
            int day = 0;
            if (txt.Text == "Mon") { day = 0; }
            else if (txt.Text == "Tue") { day = 1; }
            else if (txt.Text == "Wed") { day = 2; }
            else if (txt.Text == "Thu") { day = 3; }
            else if (txt.Text == "Fri") { day = 4; }
            else if (txt.Text == "Sat") { day = 5; }
            else if (txt.Text == "Sun") { day = 6; }
            else if (txt.Text == "Everyday") { day = 7; }
            if (dayList[day] == 1)
            {
                dayList[day] = 0;
                txt.SetTextColor(Color.LightGray);
            }
            else
            {
                dayList[day] = 1;
                txt.SetTextColor(Color.Aqua);
            }
        }
    }
}