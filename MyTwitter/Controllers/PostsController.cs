using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyTwitter.Data;
using MyTwitter.Models;

namespace MyTwitter.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostsController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }
        
        public IActionResult Index(int? userId = null)
        {
            if (userId == null)
            {
                return View(_applicationDbContext.Posts.OrderByDescending(x => x.DateCreated).Take(20).ToList());
            }
            var user = _applicationDbContext.ApplicationUsers.SingleOrDefault(x => x.Id == userId);
            if (user != null)
            {
                return View("Index",
                    _applicationDbContext.Posts.Where(x => x.ApplicationUser == user).OrderByDescending(x => x.DateCreated).Take(20).ToList());
            }
            return NotFound();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string message)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var post = new Post(){ApplicationUser = user, Message = message};
            _applicationDbContext.Posts.Add(post);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index", new {userId = user.Id});
        }
    }
}