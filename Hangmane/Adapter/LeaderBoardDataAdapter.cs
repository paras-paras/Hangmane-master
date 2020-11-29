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
using Hangmane.Data;

namespace Hangmane.Adapter
{
    public class LeaderBoardDataAdapter : BaseAdapter<LeaderBoardData>
    {

        private readonly Activity context;
        private readonly List<LeaderBoardData> datas;

        public LeaderBoardDataAdapter(Activity context, List<LeaderBoardData> datas)
        {
            this.datas = datas;
            this.context = context;
        }

        public override int Count
        {
            get { return datas.Count; }

        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override LeaderBoardData this[int position]
        {
            get { return datas[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.list_row, null, false);
            }

            TextView textUserName = row.FindViewById<TextView>(Resource.Id.textUserName);
            TextView textStat = row.FindViewById<TextView>(Resource.Id.textStat);
            TextView textScore = row.FindViewById<TextView>(Resource.Id.textScore);
            
            textUserName.Text = "User: " + datas[position].UserName;
            textScore.Text = "Total Score: " + datas[position].Score;
            string output = "Win: " + datas[position].Win + " Lose: " + datas[position].Lose;
            textStat.Text = output;

            return row;
        }
    }
}