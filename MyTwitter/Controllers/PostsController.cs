using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyTwitter.Data;
using MyTwitter.Models;
using MyTwitter.Services;

namespace MyTwitter.Controllers
{
    public class PostsController : Controller
    {
        public const string VisitsCounterKey = "visitsCounter";

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQueueClient _queueClient;
        private readonly IRedisClient _redisClient;

        public PostsController(ApplicationDbContext applicationDbContext, 
            UserManager<ApplicationUser> userManager, 
            IQueueClient queueClient, 
            IRedisClient redisClient)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _queueClient = queueClient;
            _redisClient = redisClient;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create([FromQuery]int? userId = null)
        {
            if (userId == null)
            {
                return new OkObjectResult(_applicationDbContext.Posts.OrderByDescending(x => x.DateCreated).Take(20).ToList());
            }
            var user = _applicationDbContext.ApplicationUsers.SingleOrDefault(x => x.Id == userId);
            if (user != null)
            {
                ApplicationUser currUser = HttpContext.User.Identity.IsAuthenticated ? (await _userManager.GetUserAsync(HttpContext.User)) : null;
                string counterKey = "c_" + user.Id;
                long counter = currUser == user
                    ? _redisClient.GetValue<long>(counterKey)
                    : _redisClient.IncrementValue(counterKey);
                return new OkObjectResult(_applicationDbContext.Posts.Where(x => x.ApplicationUser == user).OrderByDescending(x => x.DateCreated).Take(20).ToList());
            }
            return NotFound(); ;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string message)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var post = new Post(){ApplicationUser = user, Message = message};
            await _queueClient.Create(post);
            //_applicationDbContext.Posts.Add(post);
            //_applicationDbContext.SaveChanges();
            return new OkObjectResult(post);
        }
    }
}