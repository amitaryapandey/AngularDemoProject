using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository= blogPostRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            if (request == null)
            {
               return NotFound();
            }

            var blogPost = new BlogPost()
            {
                Title = request.Title,
                Content = request.Content,
                Author = request.Author,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                UrlHandle = request.UrlHandle
            };
            var blogPostResult =  await _blogPostRepository.CreateBlogPostAsync(blogPost);

            var blogPostDto = new BlogPost()
            {
                Id = blogPost.Id,
                Title = blogPostResult.Title,
                Content = blogPostResult.Content,
                Author = blogPostResult.Author,
                FeaturedImageUrl = blogPostResult.FeaturedImageUrl,
                IsVisible = blogPostResult.IsVisible,
                PublishedDate = blogPostResult.PublishedDate,
                ShortDescription = blogPostResult.ShortDescription,
                UrlHandle = blogPostResult.UrlHandle
            };

            return Ok(blogPostDto);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var result = await _blogPostRepository.GetAllBlogPostsAsync();
            var blogPostDto = new List<BlogPostDto>();
            foreach (var item in result)
            {
                var blogPost = new BlogPostDto();
                blogPost.Title = item.Title;
                blogPost.Content = item.Content;
                blogPost.Author = item.Author;
                blogPost.PublishedDate = item.PublishedDate;
                blogPost.UrlHandle = item.UrlHandle;
                blogPost.Id = item.Id;
                blogPost.FeaturedImageUrl = item.FeaturedImageUrl;
                blogPost.IsVisible = item.IsVisible;
                blogPost.ShortDescription= item.ShortDescription;
                blogPostDto.Add(blogPost);
            }

            return Ok(blogPostDto);
        }
    }
}
