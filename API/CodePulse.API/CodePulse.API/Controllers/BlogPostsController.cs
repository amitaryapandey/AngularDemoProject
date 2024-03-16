using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Implementation;
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
        private readonly ICategoryRepository _categoryRepository;
        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            _blogPostRepository= blogPostRepository;
            _categoryRepository = categoryRepository;
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
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };
            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetCategoryByIdAsync(categoryGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }
            var blogPostResult =  await _blogPostRepository.CreateBlogPostAsync(blogPost);

            var blogPostDto = new BlogPostDto()
            {
                Id = blogPost.Id,
                Title = blogPostResult.Title,
                Content = blogPostResult.Content,
                Author = blogPostResult.Author,
                FeaturedImageUrl = blogPostResult.FeaturedImageUrl,
                IsVisible = blogPostResult.IsVisible,
                PublishedDate = blogPostResult.PublishedDate,
                ShortDescription = blogPostResult.ShortDescription,
                UrlHandle = blogPostResult.UrlHandle,
                Categories = blogPostResult.Categories.Select(x => new CategoryDto 
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
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
                blogPost.Categories = item.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList();

                blogPostDto.Add(blogPost);
            }

            return Ok(blogPostDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var item = await _blogPostRepository.GetBlogPostById(id);

            if (item == null) { return NotFound(); }

            var blogPost = new BlogPostDto();
            blogPost.Title = item.Title;
            blogPost.Content = item.Content;
            blogPost.Author = item.Author;
            blogPost.PublishedDate = item.PublishedDate;
            blogPost.UrlHandle = item.UrlHandle;
            blogPost.Id = item.Id;
            blogPost.FeaturedImageUrl = item.FeaturedImageUrl;
            blogPost.IsVisible = item.IsVisible;
            blogPost.ShortDescription = item.ShortDescription;
            blogPost.Categories = item.Categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                UrlHandle = x.UrlHandle
            }).ToList();

            return Ok(blogPost);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            var blogPost = new BlogPost()
            {
                Id =id,
                Title = request.Title,
                Content = request.Content,
                Author = request.Author,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };
            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetCategoryByIdAsync(categoryGuid);
                if (existingCategory != null) 
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }
            var updatedBlogPostResult = await _blogPostRepository.UpdateBlogPostAsync(blogPost);
            if (updatedBlogPostResult == null)
            {
                return NotFound();
            }

            var blogPostDto = new BlogPostDto()
            {
                Id = blogPost.Id,
                Title = updatedBlogPostResult.Title,
                Content = updatedBlogPostResult.Content,
                Author = updatedBlogPostResult.Author,
                FeaturedImageUrl = updatedBlogPostResult.FeaturedImageUrl,
                IsVisible = updatedBlogPostResult.IsVisible,
                PublishedDate = updatedBlogPostResult.PublishedDate,
                ShortDescription = updatedBlogPostResult.ShortDescription,
                UrlHandle = updatedBlogPostResult.UrlHandle,
                Categories = updatedBlogPostResult.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(blogPostDto);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
           var deletedBlogPost =  await _blogPostRepository.DeleteAsync(id);
            if (deletedBlogPost == null)
            {
                return NotFound();
            }

            var blogPostDto = new BlogPostDto()
            {
                Id = deletedBlogPost.Id,
                Title = deletedBlogPost.Title,
                Content = deletedBlogPost.Content,
                Author = deletedBlogPost.Author,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                IsVisible = deletedBlogPost.IsVisible,
                PublishedDate = deletedBlogPost.PublishedDate,
                ShortDescription = deletedBlogPost.ShortDescription,
                UrlHandle = deletedBlogPost.UrlHandle,
                Categories = deletedBlogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(blogPostDto);
        }
    }
}
