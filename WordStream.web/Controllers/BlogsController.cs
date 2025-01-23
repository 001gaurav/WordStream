using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WordStream.web.Models.Domain;
using WordStream.web.Models.ViewModels;
using WordStream.web.Repositories;

namespace WordStream.web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository,
            IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlhandle)
        {
            var liked = false;
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlhandle);
            var blogDetailViewModel = new BlogDetailViewModel();

            if (blogPost != null)
            {
                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if (signInManager.IsSignedIn(User))
                {
                    // Get like for this blog for user
                    var likesForBlog = await blogPostLikeRepository.GetLikeForBlog(blogPost.Id);

                    var userId = userManager.GetUserId(User);

                    if (userId != null)
                    {
                        var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
                }

                //Get Comment for BlogPost
                var blogComments = await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);
                var blogCommentsForView = new List<BlogComment>();
                foreach (var blogComment in blogComments)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                    });
                }

                blogDetailViewModel = new BlogDetailViewModel
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    visible = blogPost.visible,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = blogCommentsForView
                };
            }

            return View(blogDetailViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailViewModel blogDetailViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {

                var domainModel = new BlogPostComment()
                {
                    BlogPostId = blogDetailViewModel.Id,
                    Description = blogDetailViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };
                await blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new { urlhandle = blogDetailViewModel.UrlHandle });
            }
            return View();

        }
    }
}
