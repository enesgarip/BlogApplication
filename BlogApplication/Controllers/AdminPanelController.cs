using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BlogApplication.Filters;
using BlogApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace BlogApplication.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogContext _context;
        public AdminPanelController(ILogger<HomeController> logger, BlogContext context)
        {
            _logger = logger;
            _context = context;
        }
        [UserFilter]
        public IActionResult Index()
        {
            var authorID=HttpContext.Session.GetInt32("id");
            var author = _context.Author.FirstOrDefault(w => w.Id ==authorID);
            ViewBag.Message = "Welcome to the system " + author.Name+ " "+ author.Surname;
            return View();
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
       
        public IActionResult Category()
        {
            List<Category> list = _context.Category.ToList();
            return View(list);
        }
        [UserFilter]
     
        public async Task<IActionResult> CategoryDetails(int Id)
        {
            var category = await _context.Category.FindAsync(Id);
            return Json(category);
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
     
        public IActionResult Author()
        {
            List<Author> list = _context.Author.ToList();
            return View(list);
        }
        [UserFilter]
       
        public async Task<IActionResult> DeleteAuthor(int? Id)
        {
            var author = await _context.Author.FindAsync(Id);
            _context.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Author));
        }
        [UserFilter]
        public IActionResult AddBlog()
        {
            ViewBag.Categories = _context.Category.Select(w =>
                new SelectListItem
                {
                    Text = w.Name,
                    Value = w.Id.ToString()
                }
            ).ToList();
            return View();
        }
        [UserFilter]
    
        public IActionResult Publish(int Id)
        {
            var blog = _context.Blog.Find(Id);
            blog.IsPublish = true;
            blog.CreateTime=DateTime.Now;
            _context.Update(blog);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [UserFilter]
        public async Task<IActionResult> Save(Blog model)
        {
           
            if (model != null)
            {
                var file = Request.Form.Files.First();
                string savePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "img");
                var fileName = $"{DateTime.Now:MMddHHmmss}.{file.FileName.Split(".").Last()}";
                var fileUrl = Path.Combine(savePath, fileName);
                using (var fileStream = new FileStream(fileUrl, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                model.ImagePath = fileName;
                model.AuthorId = (int) HttpContext.Session.GetInt32("id");
                await _context.AddAsync(model); 
                await _context.SaveChangesAsync();
                return Json(true);
                
            }
            return Json(false);
        }

        [UserFilter]
        public IActionResult Blog()
        {
            var list = _context.Blog.ToList();
            return View(list);
        }

    }
}
