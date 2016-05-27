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
    class Profile
    {
        private int ID;
        private string name;
        private int startTime;
        private int stayonTime;
        private bool isActive;
        private string days;

        public Profile() { }

        public Profile(int Id, string Name, int StartTime, int StayOnTime, int Active, string Days)
        {
            ID = Id;
            name = Name;
            startTime = StartTime;
            stayonTime = StayOnTime;
            isActive = Convert.ToBoolean(Active);
            days = Days;
        }
        public void setID(int Id)
        {
            ID = Id;
        }
        public int getID()
        {
            return ID;
        }
        public void setName(string Name)
        {
            name = Name;
        }
        public string getName()
        {
            return name;
        }
        public void setStartTime(int starttime)
        {
            startTime = starttime;
        }
        public int getStartTime()
        {
            return startTime;
        }
        public void setStayOnTime(int stayontime)
        {
            stayonTime = stayontime;
        }
        public int getStayontime()
        {
            return stayonTime;
        }
        public void setActive(int Active)
        {
            isActive = Convert.ToBoolean(Active);
        }
        public bool getActive()
        {
            return isActive;
        }
        public void setDays(string Days)
        {
            days = Days;
        }
        public string getDays()
        {
            return days;
        }
    }
    class ProfileAdapter : BaseAdapter<Profile> {
        public List<Profile> profilelist;
        private Context context;

        public ProfileAdapter(Context context, List<Profile> Profiles)
        {
            profilelist = Profiles;
            this.context = context;
        }

        public override Profile this[int position]
        {
            get{ return profilelist[position]; }
        }

        public override int Count
        {
            get
            {
                return profilelist.Count();
            }
        }

        public override long GetItemId(int position)
        {
            return profilelist[position].getID();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.ProfileOverview, null, false);
            }
            TextView txtName = row.FindViewById<TextView>(Resource.Id.profilename);
            txtName.Text = profilelist[position].getName();

            TextView txtTime = row.FindViewById<TextView>(Resource.Id.Starttime);
            txtTime.Text = Timers.getTimeFormat(profilelist[position].getStartTime(), true);
            ImageView activeImg = row.FindViewById<ImageView>(Resource.Id.Statusimg);
            if (profilelist[position].getActive())
            {
                txtName.SetTextColor(Color.Aqua);
                txtTime.SetTextColor(Color.Aqua);
                activeImg.SetImageResource(Resource.Drawable.enabledBeans);
            }
            else
                activeImg.SetImageResource(Resource.Drawable.disabledBeans);
            return row;
        }


    }
}