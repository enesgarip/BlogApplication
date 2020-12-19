using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApplication.Filters;
using BlogApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            return View();
        }

       
        public IActionResult LoginPage()
        {
            return View();
        }

        public IActionResult Login(string Email, string Password)
        {
           
            var author = _context.Author.FirstOrDefault(w => w.Email == Email && w.Password == Password);
            if (author == null)
            {
                return RedirectToAction("LoginPage", "Admin");
            }
            
            HttpContext.Session.SetInt32("id", author.Id);
            return RedirectToAction(nameof(Category));
        }
        [UserFilter]
        public async Task<IActionResult> AddCategory(Category category)
        {
            if (category.Id == 0)
            {
                await _context.AddAsync(category);
            }
            else
            {
                _context.Update(category);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Category));
        }
        [UserFilter]
        public async Task<IActionResult> AddAuthor(Author author)
        {
            if (author.Id == 0)
            {
                await _context.AddAsync(author);
            }
            else
            {
                _context.Update(author);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Author));
        }
        [UserFilter]
        public async Task<IActionResult> AuthorDetails(int Id)
        {
            var author = await _context.Author.FindAsync(Id);
            return Json(author);
        }
        [UserFilter]
        public async Task<IActionResult> CategoryDetails(int Id)
        {
            var category = await _context.Category.FindAsync(Id);
            return Json(category);
        }
        [UserFilter]
        public IActionResult Category()
        {
            List<Category> list = _context.Category.ToList();
            return View(list);
        }
        [UserFilter]
        public IActionResult Author()
        {
            List<Author> list = _context.Author.ToList();
            return View(list);
        }
        [UserFilter]
        public async Task<IActionResult> DeleteCategory(int? Id)
        {
            Category category = await _context.Category.FindAsync(Id);
            _context.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Category));
        }
        [UserFilter]
        public async Task<IActionResult> DeleteAuthor(int? Id)
        {
            var author = await _context.Author.FindAsync(Id);
            _context.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Author));
        }

        public IActionResult LogOut()
        {

            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

    }
}
