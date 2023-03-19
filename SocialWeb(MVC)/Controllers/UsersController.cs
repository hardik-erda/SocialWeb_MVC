using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialWeb_MVC_.Models;
using System.Dynamic;
using System.Reflection;

namespace SocialWeb_MVC_.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public IActionResult SignIn()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(UsersModel obj)
        {
            
                bool res;

                UsersModel userobj = new UsersModel();
                res = userobj.signIn(obj);

                if (res)
                {
                    
                    HttpContext.Session.SetString("username", obj.UserName.ToString());
                    HttpContext.Session.SetInt32("uid", userobj.getUid(obj.UserName.ToString()));
                //TempData["uid"]= userobj.getUid(obj.UserName.ToString());
                    TempData["username"] = obj.UserName.ToString();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["isSignIn"] = false;
                }
            
            return View();

        }
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "Users");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(UsersModel obj)
        {
            //add for file upload
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/NewFolder/ProfilePics");
            FileInfo fileInfo = new FileInfo(obj.File.FileName);
            

            bool res;
            UsersModel userobj = new UsersModel();
            res = userobj.SignUp(obj);

            if (res)
            {
                //add for file upload

                int uid = obj.getUid(obj.UserName);
                string fileName = uid + fileInfo.Extension;

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    obj.File.CopyTo(stream);
                }
                if(obj.updateProfileImgPath(fileName,uid))
                {
                    return RedirectToAction("SignIn", "Users");
                }

            }
            else
            {
                TempData["isSignUp"] = false;
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddPost()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPost(PostModel obj)
        {
            bool res;
            obj.Uid=Convert.ToInt32(HttpContext.Session.GetInt32("uid"));
            //obj.Uid = Convert.ToInt32(TempData["uid"]);
            PostModel userobj = new PostModel();
            res = userobj.AddPost(obj);

            if (res)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["isAddPost"] = false;
            }
            return View();
        }
        [HttpGet]

        public IActionResult Profile() 
        {
            UsersModel userobj = new UsersModel();
            //TempData["uid"] = userobj.getUid(HttpContext.Session.GetString("username"));
            TempData["uid"] = HttpContext.Session.GetInt32("uid");
            TempData["username"] = HttpContext.Session.GetString("username");
            PostModel obj1 = new PostModel();
            List<PostModel> lstobj1 = obj1.getPostData(Convert.ToInt32(HttpContext.Session.GetInt32("uid")), HttpContext.Session.GetString("username"));
            FriendsModel obj = new FriendsModel();
            List<FriendsModel> lst = obj.countFollow(Convert.ToInt32(HttpContext.Session.GetInt32("uid")));
            dynamic myModel = new ExpandoObject();
            myModel.numpost = lstobj1;
            myModel.numFollow = lst;
            return View(myModel);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult EditProfile(UsersModel obj)
        {
            obj.Uid = HttpContext.Session.GetInt32("uid");
            bool res = obj.updateUser(obj);
            if(res)
            {
                
                HttpContext.Session.SetString("username", obj.UserName);
                return RedirectToAction("Profile","Users");
            }
            return View();
        }

        public IActionResult FollowerList()
        {
            FriendsModel obj = new FriendsModel();
            List <FriendsModel> lst= obj.getFollowersList(Convert.ToInt32(HttpContext.Session.GetInt32("uid")));
            return View(lst);
        }
        public IActionResult FollowingList()
        {
            FriendsModel obj = new FriendsModel();
            List<FriendsModel> lst = obj.getFollowingList(Convert.ToInt32(HttpContext.Session.GetInt32("uid")));
            return View(lst);
        }

        
        public IActionResult SearchList(UsersModel obj)
        {
            UsersModel obj2 = new UsersModel();
            obj.Uid = HttpContext.Session.GetInt32("uid");
            TempData["uid"] = HttpContext.Session.GetInt32("uid");
            List<UsersModel> lst = obj2.getSearchList(obj);
            return View(lst);
        }

        public IActionResult Unfollow(int id)
        {
            FriendsModel obj = new FriendsModel();
            bool res =obj.UnfollowFromList(Convert.ToInt32(HttpContext.Session.GetInt32("uid")),id);
            if(res)
            {
                return RedirectToAction("Profile", "Users");
                
            }
             return View();
            
        }
        public IActionResult follow(int id)
        {
            FriendsModel obj = new FriendsModel();
            bool res = obj.followFromList(Convert.ToInt32(HttpContext.Session.GetInt32("uid")), id);
            if (res)
            {
                return RedirectToAction("Profile", "Users");

            }
            return View();

        }
        public IActionResult LikePost(int id)
        {
            PostModel obj = new PostModel();
            bool res = obj.LikePost(id);
            if (res)
            {
                return RedirectToAction("Index", "Home");

            }
            return View();

        }
    }
}
