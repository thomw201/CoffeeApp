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
using Java.Lang;
using Java.Util;
using Android.Text.Format;

namespace CoffeeApp
{
    public class TimePickerFragment : DialogFragment, TimePickerDialog.IOnTimeSetListener
    {
        TextView timeLbl;

        public TimePickerFragment(TextView timeLabel)
        {
            timeLbl = timeLabel;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            Calendar c = Calendar.Instance;
            return new TimePickerDialog(Activity, this, Timers.getHour(timeLbl.Text), Timers.getMinutes(timeLbl.Text), DateFormat.Is24HourFormat(Activity));
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            timeLbl.Text = Timers.getTimeFormat(hourOfDay, minute, true);
        }
    }
}