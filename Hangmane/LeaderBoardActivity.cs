using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Hangmane.Adapter;
using Hangmane.Data;

namespace Hangmane
{
    [Activity(Label = "Leader Board", Theme = "@style/AppTheme")]
    public class LeaderBoardActivity : AppCompatActivity
    {
        ListView listLeaderBoard;
        DBManager manager;
        LeaderBoardDataAdapter adapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.leader_layout);
            manager = new DBManager();
            // Create your application here
            listLeaderBoard = FindViewById<ListView>(Resource.Id.listLeaderBoard);
            adapter = new LeaderBoardDataAdapter(this, manager.GetLeaderBoardData());
            listLeaderBoard.Adapter = adapter;
        }
    }
}