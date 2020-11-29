using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Hangmane
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class SpashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.splash_layout);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Wait For 3 Seconds and Then Start The Main Activity
        async void SimulateStartup()
        {
            // Delay of 3 Seconds
            await Task.Delay(3000); 
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            Finish();
        }
    }
}