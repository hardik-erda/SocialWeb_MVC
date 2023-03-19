using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;

namespace SocialWeb_MVC_.Models
{
    public class FriendsModel
    {
        public int followers { get; set; }
        public string followerName { get; set; }
        public string ProFileImg { get; set; }
        public int following { get; set; }
        public int numPost { get; set; }

        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\Sem 6\\ASP\\SocialWeb(MVC)\\SocialWeb(MVC)\\App_Data\\db_socialMedia.mdf\";Integrated Security=True");
        public List<FriendsModel> countFollow(int uid)
        {
            //for followers count
            SqlCommand cmd = new SqlCommand("select count(FollowingId) as count from Friends where FollowingId = @uid", con);
            cmd.Parameters.AddWithValue("@uid", uid);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            int follower;
            if (dr.Read())
            {

                follower =Convert.ToInt32( dr["count"]);
            }
            else
            {
                follower = 0;
            }
            dr.Close();

            //for following count
            cmd = new SqlCommand("select count(FollowingId) as count from Friends where Uid = @uid", con);
            cmd.Parameters.AddWithValue("@uid", uid);
            dr = cmd.ExecuteReader();
            int numFollowing;
            if (dr.Read())
            {

                numFollowing = Convert.ToInt32(dr["count"]);
            }
            else
            {
                numFollowing = 0;
            }
            dr.Close();

            //for following count
            cmd = new SqlCommand("select count(pid) as count from Posts where Uid = @uid", con);
            cmd.Parameters.AddWithValue("@uid", uid);
            dr = cmd.ExecuteReader();
            int postCount;
            if (dr.Read())
            {

                postCount = Convert.ToInt32(dr["count"]);
            }
            else
            {
                postCount = 0;
            }
            dr.Close();

            List<FriendsModel> lst = new List<FriendsModel>();
            lst.Add(new FriendsModel
            {
                followers = follower,
                following = numFollowing,
                numPost = postCount
            });
            return lst;
        }
        public List<FriendsModel> getFollowingList(int uid)
        {
            List<FriendsModel> lst = new List<FriendsModel>();
            //SqlCommand cmd = new SqlCommand("select * from Friends where Uid = @uid ", con);
            SqlCommand cmd = new SqlCommand("select f.followingId,u.UserName,u.ProfileImg from Friends as f INNER JOIN Users as u ON f.FollowingId=u.UserId where f.Uid = @uid ", con);
            cmd.Parameters.AddWithValue("@uid", uid);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lst.Add(new FriendsModel
                {
                    followers = Convert.ToInt32(dr["FollowingId"]),
                    followerName = dr["UserName"].ToString(),
                    ProFileImg = dr["ProfileImg"].ToString()
                }) ;
            }
            con.Close();
            return lst;
        }
        public List<FriendsModel> getFollowersList(int uid)
        {
            List<FriendsModel> lst = new List<FriendsModel>();
            //SqlCommand cmd = new SqlCommand("select * from Friends where Uid = @uid ", con);
            SqlCommand cmd = new SqlCommand("select f.Uid,u.UserName,u.ProfileImg from Friends as f INNER JOIN Users as u ON f.Uid=u.UserId where f.followingId = @uid ", con);
            cmd.Parameters.AddWithValue("@uid", uid);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lst.Add(new FriendsModel
                {
                    followers = Convert.ToInt32(dr["Uid"]),
                    followerName = dr["UserName"].ToString(),
                    ProFileImg = dr["ProfileImg"].ToString()
                });
            }
            con.Close();
            return lst;
        }

        public bool UnfollowFromList(int uid,int fid)
        {
            SqlCommand cmd = new SqlCommand("delete from Friends where Uid=@uid AND FollowingId=@fid", con);
            cmd.Parameters.AddWithValue("@uid", uid);
            cmd.Parameters.AddWithValue("@fid", fid);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            if(i>=1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool followFromList(int uid, int fid)
        {
            SqlCommand cmd = new SqlCommand("insert into Friends(Uid,FollowingId) values(@uid,@fid)", con);
            cmd.Parameters.AddWithValue("@uid", uid);
            cmd.Parameters.AddWithValue("@fid", fid);
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
