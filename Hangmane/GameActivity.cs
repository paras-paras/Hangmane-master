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
using Hangmane.Logic;
using Java.Interop;

namespace Hangmane
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class GameActivity : Activity
    {
        string username;
        WordList list;
        TextView textGuess,textScore,textMaxScore, textWrongAttempt;
        HangmanLogic logic;
        int wrong;
        DBManager manager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.game_layout);
            manager = new DBManager();
            list = new WordList(this);
            logic = new HangmanLogic(list.GetRandomWord());
            username = Intent.GetStringExtra("UserName");
            textGuess = FindViewById<TextView>(Resource.Id.textGuess);
            textScore = FindViewById<TextView>(Resource.Id.textScore);
            textMaxScore = FindViewById<TextView>(Resource.Id.textMaxScore);
            textWrongAttempt = FindViewById<TextView>(Resource.Id.textWrongAttempt);
            textGuess.Text = logic.GetGuessString();
            textMaxScore.Text = " Max Score: " + logic.GetWordPoints();
            wrong = logic.WrongAllowed();
            textWrongAttempt.Text = " Remaining Wrong Attempt: " + wrong;
        }

        [Export("ButtonClick")]
        public void ButtonClick(View view)
        {
            if(view is Button)
            {
                Button btn = view as Button;
                if(btn.Enabled)
                {
                    char ch = btn.Text[0];
                    btn.Enabled = false;
                    if (logic.ProcessCharacter(ch))
                    {
                        textGuess.Text = logic.GetGuessString();                        
                        textScore.Text = " Your Score: " + logic.GetPlayerPoints();
                        if(logic.Compare())
                        {
                            ProcessResult();
                        }
                    }
                    else
                    {
                        wrong--;
                        if(wrong == -1 )
                        {
                            ProcessResult();
                        }
                        else
                        {
                            textWrongAttempt.Text = " Remaining Wrong Attempt: " + wrong;
                        }
                        
                    }
                }                
            }
        }

        public void ProcessResult()
        {
            UserGames userGames = new UserGames();
            userGames.TotalScore = logic.GetPlayerPoints();
            userGames.UserName = username;
            if (logic.Compare())
            {
                userGames.GameStatus = "WIN";
            }
            else
            {
                userGames.GameStatus = "LOSE";
            }
            manager.AddNewUserGame(userGames);
            AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog alert = dialog.Create();
            alert.SetTitle("Hangman");
            alert.SetMessage("Click OK to Process Result");
            alert.SetIcon(Resource.Drawable.abc_btn_borderless_material);
            alert.SetButton("OK", (c, ev) =>
            {
                Intent intent = new Intent(this, typeof(ResultActivity));
                intent.PutExtra("Status", userGames.GameStatus);
                intent.PutExtra("UserName", username);
                StartActivity(intent);
                Finish();
            });
            alert.Show();
        }
    }
}