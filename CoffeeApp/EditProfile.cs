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
    [Activity(Label = "Edit Profile", Icon = "@drawable/applogo")]
    public class EditProfile : Activity
    {
        private int profileID;
        private string name;
        private int time;
        private int stayontime;
        private bool active;
        private List<char> dayList;
        private EditText profileName;
        private TextView profileTime;
        private TextView profileStayOnTime;
        private CheckBox activeChkbx;
        private int stayOnTimeCounter = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.EditProfile);
            dayList = new List<char>();
            if (this.Intent.Extras.ContainsKey("profileID"))
            {
                profileID = (int)this.Intent.Extras.Get("profileID");
                name = (string)this.Intent.Extras.Get("profileName");
                time = (int)this.Intent.Extras.Get("profileTime");
                stayontime = (int)this.Intent.Extras.Get("profileStayontime");
                active = (bool)this.Intent.Extras.Get("profileActive");
                string dayString = (string)this.Intent.Extras.Get("profileDays");
                foreach (char day in dayString)
                {
                    dayList.Add(day);
                }
            }
            //set variables in the textboxes
            profileName = FindViewById<EditText>(Resource.Id.profilenameEditText);
            profileName.Text = Convert.ToString(name);
            profileTime = FindViewById<TextView>(Resource.Id.timeLabel);
            profileTime.Text = Timers.getTimeFormat(time, true);
            profileStayOnTime = FindViewById<TextView>(Resource.Id.StayontimeTxtLabel);
            profileStayOnTime.Text = Timers.getFriendlyMinutes(stayontime);
            profileStayOnTime.SetTextColor(Color.Aqua);
            activeChkbx = FindViewById<CheckBox>(Resource.Id.ActiveCheckbox);
            activeChkbx.Checked = active;
            //save and cancel button trigger events
            Button SaveButton = FindViewById<Button>(Resource.Id.saveButton);
            SaveButton.Click += saveButtonClick;
            Button CancelButton = FindViewById<Button>(Resource.Id.cancelButton);
            CancelButton.Click += cancelButtonClick;

            profileTime.TextSize = 20;
            profileTime.SetTextColor(Color.Aqua);
            profileTime.Click += delegate
            {
                TimePickerFragment pickTime = new TimePickerFragment(profileTime);
                pickTime.Show(this.FragmentManager, "Starttime");
            };
            profileStayOnTime.Click += delegate
            {
                if (stayOnTimeCounter == Timers.StayOnTimes.Length)
                {
                    stayOnTimeCounter = 0;
                }
                stayontime = Timers.StayOnTimes[stayOnTimeCounter];
                profileStayOnTime.Text = Timers.getFriendlyMinutes(stayontime);
                stayOnTimeCounter++;
            };
            //profileStayOnTime.LongClick += delegate
            //{
            //    var dialog = new NumberPickerDialogFragment();
            //    dialog.Show(FragmentManager, "number");
            //};


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

            foreach (TextView daybutton in dayButtons)
            {
                daybutton.Click += dayClicked;
            }
            //activate daybuttons like the booleans
            for (int i = 0; i < dayList.Count; i++)
            {
                if (dayList[i] == '1')
                {
                    dayButtons[i].SetTextColor(Color.Aqua);
                }
                else
                    dayButtons[i].SetTextColor(Color.LightGray);
            }
        }

        private void cancelButtonClick(object sender, EventArgs e)
        {
            StartActivity(typeof(Profiles));
            this.Finish();
        }

        private void saveButtonClick(object sender, EventArgs e)
        {
            int timeResult;
            if (int.TryParse((profileTime.Text).Replace(":", ""), out timeResult))
            {
                string profiledata = String.Format("{5} {0},{1},{2},{3},{4},", profileID, profileName.Text, timeResult, stayontime, Convert.ToInt32(activeChkbx.Checked), Resources.GetString(Resource.String.updateprofilecode));
                for (int i = 0; i < dayList.Count; i++)
                {
                    profiledata += dayList[i];
                }
                profiledata += ',';
                Communication.Send(profiledata);
                StartActivity(typeof(Profiles));
                this.Finish();
            }
            else
                Toast.MakeText(this, "Enter all fields before saving.", ToastLength.Short).Show();

        }

       

        private void dayClicked(object sender, EventArgs e)
        {
            TextView txt = sender as TextView;
            int day = 0 ;
            if (txt.Text == "Mon") { day = 0; }
            else if (txt.Text == "Tue") { day = 1; }
            else if (txt.Text == "Wed") { day = 2; }
            else if (txt.Text == "Thu") { day = 3; }
            else if (txt.Text == "Fri") { day = 4; }
            else if (txt.Text == "Sat") { day = 5; }
            else if (txt.Text == "Sun") { day = 6; }
            else if (txt.Text == "Everyday") { day = 7; }
            if (dayList[day] == '1')
            {
                dayList[day] = '0';
                txt.SetTextColor(Color.LightGray);
            }
            else
            {
                dayList[day] = '1';
                txt.SetTextColor(Color.Aqua);
            }
        }
    }

}