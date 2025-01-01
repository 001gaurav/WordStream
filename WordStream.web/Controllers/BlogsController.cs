using Microsoft.AspNetCore.Mvc;
using WordStream.web.Repositories;

namespace WordStream.web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogsController(IBlogPostRepository blogPostRepository) 
        {
            this.blogPostRepository = blogPostRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlhandle)
        {
            var BlogPost = await blogPostRepository.GetByUrlHandleAsync(urlhandle);
            return View(BlogPost);
        }
    }
}
