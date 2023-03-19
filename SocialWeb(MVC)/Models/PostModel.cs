using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Drawing;

namespace SocialWeb_MVC_.Models
{
    public class PostModel
    {
        public int Uid { get; set; }
        public int Pid { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public string Description { get; set; }
        public string Likes { get; set; }

        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\Sem 6\\ASP\\SocialWeb(MVC)\\SocialWeb(MVC)\\App_Data\\db_socialMedia.mdf\";Integrated Security=True");

        public bool AddPost(PostModel obj)
        {
            SqlCommand cmd = new SqlCommand("Insert into Posts(Uid,PostTitle,PostDes) values(@Uid,@Title,@Des)", con);
            cmd.Parameters.AddWithValue("@uid",obj.Uid);
            cmd.Parameters.AddWithValue("@Title", obj.Title);
            cmd.Parameters.AddWithValue("@Des", obj.Description);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i == 1;

            //if(i == 1)
            //{
            //    return true;
            //}
            //return false;
        }
        public List<PostModel> getData(int uid) 
        {
            List<PostModel> lst = new List<PostModel>();
            //SqlCommand cmd = new SqlCommand("select * from Posts where Uid != @uid", con);
            SqlCommand cmd = new SqlCommand("select * from Posts INNER JOIN Users ON Posts.Uid = Users.UserId where Posts.Uid != @uid order by Posts.Pid desc", con);
            cmd.Parameters.AddWithValue("@uid", uid);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                lst.Add(new PostModel
                {
                    Uid = Convert.ToInt32(dr["Uid"]),
                    Pid = Convert.ToInt32(dr["Pid"]),
                    Name = dr["UserName"].ToString(),
                    Title = dr["PostTitle"].ToString(),
                    Img = dr["PostImg"].ToString(),
                    Description = dr["PostDes"].ToString(),
                    Likes = dr["PostLikes"].ToString()
                }) ;
            }
            con.Close();
            return lst; 
        }
        public List<PostModel> getPostData(int uid, string uname)
        {
            List<PostModel> lst = new List<PostModel>();
            
            SqlCommand cmd = new SqlCommand("select * from Posts where Uid = @uid order by Pid desc", con);
            cmd.Parameters.AddWithValue("@uid", uid);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lst.Add(new PostModel
                {
                    Uid = Convert.ToInt32(dr["Uid"]),
                    //Name = dr["UserName"].ToString(),
                    Name = uname,
                    Title = dr["PostTitle"].ToString(),
                    Img = dr["PostImg"].ToString(),
                    Description = dr["PostDes"].ToString(),
                    Likes = dr["PostLikes"].ToString()
                });
            }
            con.Close();
            return lst;
        }
        public bool LikePost(int pid)
        {
            SqlCommand cmd = new SqlCommand("update Posts set PostLikes =(select PostLikes from Posts where Pid = @pid)+1  where Pid = @pid", con);
            cmd.Parameters.AddWithValue("@pid", pid);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
