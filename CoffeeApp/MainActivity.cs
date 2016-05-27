using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Text;

namespace CoffeeApp
{
    [Activity(Label = "CoffeeApp", MainLauncher = true, Icon = "@drawable/applogo")]

    public class MainActivity : Activity
    {
        Button ConnectButton;
        EditText IPtxt;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            //move off login page if connected
            if (Communication.isConnected)
            {
                StartActivity(typeof(Profiles));
                this.Finish();
            }
            // Get our button from the layout resource,
            // and attach an event to it
            ConnectButton = FindViewById<Button>(Resource.Id.ConnectButton);
            IPtxt = FindViewById<EditText>(Resource.Id.IPeditText);
            //retreive previous used IP address
            var prefs = Application.Context.GetSharedPreferences("CoffeeApp", FileCreationMode.Private);
            IPtxt.Text = prefs.GetString("IP", null);
            //attach event when button is clicked
            ConnectButton.Click += connectButtonClick;
            IPtxt.AfterTextChanged += new EventHandler<AfterTextChangedEventArgs>(OnTextChange);
        }

        void connectButtonClick(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Connecting...", ToastLength.Short).Show();
            ConnectButton.Enabled = false;
            IPtxt.Enabled = false;
            if (!Communication.connect(IPtxt.Text))
            {
                Toast.MakeText(this, "Failed to connect.", ToastLength.Short).Show();
                ConnectButton.Enabled = true;
                IPtxt.Enabled = true;
            }
            else {

                Communication.Send(Resources.GetString(Resource.String.connectcode));
                Toast.MakeText(this, "Connected!", ToastLength.Short).Show();
                //save IP
                var prefs = Application.Context.GetSharedPreferences("CoffeeApp", FileCreationMode.Private);
                var prefEditor = prefs.Edit();
                prefEditor.PutString("IP", IPtxt.Text);
                prefEditor.Commit();
                StartActivity(typeof(CoffeeOverview));
                this.Finish();
            }
        }
        public void OnTextChange(object sender, EventArgs e)
        {
            if (IPtxt.Text.Length < 8)
                ConnectButton.Enabled = false;
            else
                ConnectButton.Enabled = true;
        }
    }
}

