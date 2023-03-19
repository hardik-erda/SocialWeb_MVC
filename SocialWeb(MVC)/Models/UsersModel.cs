using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace SocialWeb_MVC_.Models
{
    public class UsersModel
    {
        public int? Uid { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string ProfilePic { get; set; }

        //add for file upload 
        public string FileName { get; set; }
        public IFormFile File { get; set; }

        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\Sem 6\\ASP\\SocialWeb(MVC)\\SocialWeb(MVC)\\App_Data\\db_socialMedia.mdf\";Integrated Security=True;MultipleActiveResultSets=true");

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
                dr.Close();
                return true;
                //int i = cmd.ExecuteNonQuery();
            }
            dr.Close();
            cmd.Cancel();
            con.Close();
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

            cmd.Cancel();
            con.Close();
            return i == 1;

            //if(i == 1)
            //{
            //    return true;
            //}
            //return false;
        }

        public int getUid(string UserName)
        {
            SqlCommand cmd = new SqlCommand("Select * from Users where UserName = @UserName", con);

            //return obj.name;
            cmd.Parameters.AddWithValue("@UserName", UserName);

            if(con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            //con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while(dr.Read())
                {
                return Convert.ToInt32( dr["UserId"]);
                }
                //int i = cmd.ExecuteNonQuery();
            }
            dr.Close();
            cmd.Cancel();
            con.Close();
            return 0;
        }
        public List<UsersModel> getSearchList(UsersModel obj)
        {
            SqlCommand cmd = new SqlCommand("select UserId,UserName,ProfileImg from Users where UserName Like '%"+ obj.UserName + "%' and UserId not in (select FollowingId from Friends where Uid=@uid)", con);
            con.Open();
            cmd.Parameters.AddWithValue("@uid", obj.Uid);
            SqlDataReader dr = cmd.ExecuteReader();

            List<UsersModel> lst = new List<UsersModel>();
            while(dr.Read())
            {

                lst.Add(new UsersModel
                {
                    Uid = Convert.ToInt32(dr["UserId"]),
                    UserName = dr["UserName"].ToString(),
                    ProfilePic = dr["ProfileImg"].ToString()
                });
            }
            dr.Close();
            cmd.Cancel();
            con.Close();    
            return lst;
        }
        public bool updateUser(UsersModel obj)
        {
            SqlCommand cmd = new SqlCommand("update Users set UserName=@uname where UserId = @uid",con);
            cmd.Parameters.AddWithValue("@uname",obj.UserName);
            cmd.Parameters.AddWithValue("@uid", obj.Uid);
            con.Open();
            int i =cmd.ExecuteNonQuery();
            cmd.Cancel();
            con.Close();   
            if (i == 1)
            {
                return true;
            }
            return false;
        }
        public bool updateProfileImgPath(string filename,int uid)
        {
            SqlCommand cmd = new SqlCommand("update Users set ProfileImg='ProfilePics/"+filename +"'where UserId = @uid", con);
            cmd.Parameters.AddWithValue("@uid", uid);
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            int i = cmd.ExecuteNonQuery();
            cmd.Cancel();
            con.Close();
            if (i == 1)
            {
                return true;
            }
            return false;
        }
    }
}
