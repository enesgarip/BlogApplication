using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApplication.Filters;
using BlogApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogApplication.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogContext _context;
        
        public AdminController(ILogger<HomeController> logger, BlogContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/AdminPanel/Index");
            }
            return View();
        }


        public IActionResult LoginPage(string Email, string Password)
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/AdminPanel/Index");
            }
            var author = _context.Author.FirstOrDefault(w => w.Email == Email && w.Password == Password);
            if (author == null)
            {
                return View();
            }
            else
            {
                HttpContext.Session.SetInt32("id", author.Id);
                return RedirectToAction("Index", "AdminPanel");
            }
        }
      

        public IActionResult LogOut()
        {

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
