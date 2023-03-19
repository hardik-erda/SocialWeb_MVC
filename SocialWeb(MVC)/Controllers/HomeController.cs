﻿using Microsoft.AspNetCore.Mvc;
using SocialWeb_MVC_.Models;
using System.Diagnostics;

namespace SocialWeb_MVC_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("SignIn", "Users");
            }
            PostModel obj = new PostModel();
            List<PostModel> lstobj = obj.getData(Convert.ToInt32(HttpContext.Session.GetInt32("uid")));
            return View(lstobj);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}