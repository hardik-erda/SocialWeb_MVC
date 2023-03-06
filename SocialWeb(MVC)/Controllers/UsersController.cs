using Microsoft.AspNetCore.Mvc;
using SocialWeb_MVC_.Models;

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
                    TempData["uid"]= userobj.getUid(obj.UserName.ToString());                   
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
            bool res;

            UsersModel userobj = new UsersModel();
            res = userobj.SignUp(obj);

            if (res)
            {
                
                return RedirectToAction("SignIn", "Users");
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
            obj.Uid = Convert.ToInt32(TempData["uid"]);
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
    }
}
