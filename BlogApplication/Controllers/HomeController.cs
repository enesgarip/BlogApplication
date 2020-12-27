using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogApplication.Models;
using Microsoft.AspNetCore.Http;
using PagedList;
using PagedList.Mvc;
namespace BlogApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogContext _context;
        public HomeController(ILogger<HomeController> logger,BlogContext context)
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
            var list = _context.Blog.OrderByDescending(x=>x.CreateTime).Take(4).Where(b=>b.IsPublish).ToList();
            foreach (var blog in list)
            {
                blog.Author = _context.Author.Find(blog.AuthorId);
            }
            return View(list);
        }

        public IActionResult Privacy()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/AdminPanel/Index");
            }
            return View();
        }
        public IActionResult Contact()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/AdminPanel/Index");
            }
            return View();
        }
        public IActionResult Post(int Id)
        {
            var blog = _context.Blog.Find(Id);
            blog.Author = _context.Author.Find(blog.AuthorId);
            blog.ImagePath = "/img/" + blog.ImagePath;
            return View(blog);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult OlderPosts(int? page)
        {
            var list = _context.Blog.OrderByDescending(x => x.CreateTime).Where(b => b.IsPublish).ToList();
            foreach (var blog in list)
            {
                blog.Author = _context.Author.Find(blog.AuthorId);
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
            
        }
    }
}
