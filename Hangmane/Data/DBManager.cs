using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace Hangmane.Data
{
    public class DBManager
    {
        private SQLiteConnection conn;

        public DBManager()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            conn = new SQLiteConnection(Path.Combine(path, "data.db"));
            if(!CheckUserTableExists())
            {
                conn.CreateTable<User>();
                conn.CreateTable<UserGames>();
            }
            
        }

        public bool AddNewUser(User user)
        {
            try
            {
                conn.Insert(user);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool AddNewUserGame(UserGames game)
        {
            try
            {
                conn.Insert(game);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Check existing User
        public bool CheckUser(string username,string password)
        {
            List<User> users = conn.Query<User>("Select * from User");
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].UserName.Equals(username) && users[i].Password.Equals(password))
                {
                    return true;
                }

            }
            return false;
        }

        public List<LeaderBoardData> GetLeaderBoardData()
        {
            List<LeaderBoardData> datas = new List<LeaderBoardData>();
            try
            {
                List<User> users = conn.Query<User>("Select * from User");
                for(int index = 0; index < users.Count; index++)
                {
                    datas.Add(GetLeaderBoardData(users[index].UserName));
                }
            }
            catch (Exception ex) 
            {
            }
            if(datas.Count > 0)
            {
                for(int i = 0; i < datas.Count - 1; i++ )
                {
                    for(int j = 0; j < datas.Count - i - 1; j++)
                    {
                        if( datas[j].Score < datas[j+1].Score)
                        {
                            LeaderBoardData temp = datas[j];
                            datas[j] = datas[j + 1];
                            datas[j + 1] = temp;
                        }
                    }
                }
            }
            return datas;
        }

        public LeaderBoardData GetLeaderBoardData(string username)
        {
            LeaderBoardData data = new LeaderBoardData();
            data.UserName = username;
            try
            {
                string query = "Select * from UserGames Where UserName='" + username + "'";
                List<UserGames> games = conn.Query<UserGames>(query);
                for( int index = 0; index < games.Count; index++)
                {
                    data.Score += games[index].TotalScore;
                    if(games[index].GameStatus.Equals("WIN"))
                    {
                        data.Win += 1;
                    }
                    else
                    {
                        data.Lose += 1;
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return data;
        }

        private bool CheckUserTableExists()
        {
            try
            {
                conn.Get<User>(1);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}