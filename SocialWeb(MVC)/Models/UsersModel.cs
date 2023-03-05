﻿using System.Data.SqlClient;

namespace SocialWeb_MVC_.Models
{
    public class UsersModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        //public string ProfilePic { get; set; }

        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\Sem 6\\ASP\\SocialWeb(MVC)\\SocialWeb(MVC)\\App_Data\\db_socialMedia.mdf\";Integrated Security=True");

        public bool signIn(UsersModel obj)
        {
            SqlCommand cmd = new SqlCommand("Select * from Users where UserName = @UserName and Password=@Password", con);

            //return obj.name;
            cmd.Parameters.AddWithValue("@UserName", obj.UserName);
            cmd.Parameters.AddWithValue("@Password", obj.Password);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {

                return true;
                //int i = cmd.ExecuteNonQuery();
            }
            ////if (i >= 1)
            ////{
            ////    return true;
            ////}
            return false;

        }
        public bool SignUp(UsersModel obj)
        {
            SqlCommand cmd = new SqlCommand("Insert into Users(UserName,Password) values(@UserName,@Password)", con);

            cmd.Parameters.AddWithValue("@UserName", obj.UserName);
            cmd.Parameters.AddWithValue("@Password", obj.Password);
            con.Open();
            int i = cmd.ExecuteNonQuery();

            return i == 1;

            //if(i == 1)
            //{
            //    return true;
            //}
            //return false;
        }
    }
}
