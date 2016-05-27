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
    static class Timers
    {
        static int[] stayOnTimes = { 0, 10, 15, 20, 30, 45, 60, 90, 120, 150, 180, 210, 240, 300, 330, 360 };
        public static int overViewUpdateInterval = 3000;

        public static int[] StayOnTimes
        {
            get { return Timers.stayOnTimes; }
            set { Timers.stayOnTimes = value; }
        }

        public static int getHour(string time)
        {
            string _time = time;
            if (time.Contains(':'))
            {
                _time = time.Replace(":", "");
            }
            return int.Parse(_time) / 100;
        }
        public static int getMinutes(string time)
        {
            string _time = time;
            if (time.Contains(':'))
            {
                _time = time.Replace(":", "");
            }
            return int.Parse(_time) - (getHour(_time) * 100);
        }
        /// <summary>
        /// transform integers 17, 00 to "17:00"
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static string getTimeFormat(int hour, int minute, bool colon)
        {
            string colonStr = "";
            if (colon)
            {
                colonStr = ":";
            }
            if (minute < 10) // if minute is less than 10, add a zero to prevent 1 digit minute display
            {
                return String.Format("{0}{2}0{1}", hour, minute, colonStr);
            }
            else
                return String.Format("{0}{2}{1}", hour, minute, colonStr);
        }
        /// <summary>
        /// convert timestring "1700" to "17:00"
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string getTimeFormat(int time, bool colon)
        {
            int hour = time / 100;
            int minute = time - (hour * 100);
            return getTimeFormat(hour, minute, colon);
        }

        public static string getFriendlyMinutes(int minutes)
        {
            if (minutes > 59)
            {
                if (minutes % 60 == 0)
                {
                    if (minutes / 60 == 1)
                    {
                        return String.Format("{0} hour", minutes / 60);
                    }
                    return String.Format("{0} hours", minutes / 60);
                }
                return String.Format("{0} hours and {1} minutes", (int)minutes / 60, minutes % 60);
            }
            else
            {
                return String.Format("{0} minutes", minutes);
            }
        }
    }
}