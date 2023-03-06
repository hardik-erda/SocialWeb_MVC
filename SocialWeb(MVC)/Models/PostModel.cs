using System.Data.SqlClient;
using System.Drawing;

namespace SocialWeb_MVC_.Models
{
    public class PostModel
    {
        public int Uid { get; set; }
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

            return i == 1;

            //if(i == 1)
            //{
            //    return true;
            //}
            //return false;
        }
    }
}
