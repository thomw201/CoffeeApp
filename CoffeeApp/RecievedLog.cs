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
    class ReceivedLog
    {
        private int ID;
        private string time;
        private string logType;
        private string msg;

        public ReceivedLog() { }

        public ReceivedLog(int Id, string Time, int Logtype, string Msg)
        {
            ID = Id;
            time = Time;
            setLogtype(Logtype);
            msg = Msg;
        }

        public int getID()
        {
            return ID;
        }
        public void setID(int Id)
        {
            ID = Id;
        }
        public string getTime()
        {
            return time;
        }
        public void setTime(string Time)
        {
            time = Time;
        }
        public string getlogType()
        {
            return logType;
        }
        public void setLogtype(int logtype)
        {
            switch (logtype)
            {
                case 1:
                    logType = "Info";
                    break;
                case 2:
                    logType = "Warning";
                    break;
                case 3:
                    logType = "Error";
                    break;
                case 4:
                    logType = "Debug";
                    break;
            }
        }
        public string getMsg()
        {
            return msg;
        }
        public void setMsg(string Msg)
        {
            msg = Msg;
        }
    }
    class LogAdapter : BaseAdapter<ReceivedLog>
    {
        public List<ReceivedLog> logList;
        private Context context;

        public LogAdapter(Context context, List<ReceivedLog> logs)
        {
            logList = logs;
            this.context = context;
        }

        public override ReceivedLog this[int position]
        {
            get { return logList[position]; }
        }

        public override int Count
        {
            get
            {
                return logList.Count();
            }
        }

        public override long GetItemId(int position)
        {
            return logList[position].getID();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.LogOverview, null, false);
            }

            TextView logId = row.FindViewById<TextView>(Resource.Id.logid);
            logId.Text = logList[position].getID().ToString();
            TextView logType = row.FindViewById<TextView>(Resource.Id.logtype);
            logType.Text = logList[position].getlogType();
            TextView logTime = row.FindViewById<TextView>(Resource.Id.logtime);
            logTime.Text = logList[position].getTime();
            TextView logMsg = row.FindViewById<TextView>(Resource.Id.logmsg);
            logMsg.Text = logList[position].getMsg();
            return row;
        }
    }
}