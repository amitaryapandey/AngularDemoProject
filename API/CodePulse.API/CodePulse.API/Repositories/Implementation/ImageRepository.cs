using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _applicationDbContext;

        public ImageRepository(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<BlogImage>> GetBlogImages()
        {
            return await _applicationDbContext.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            try
            {
                var localPath = Path.Combine(_env.ContentRootPath, "Images", $"{file.FileName}{blogImage.FileExtension}");
                using var stream = new FileStream(localPath, FileMode.Create);
                await file.CopyToAsync(stream);
                var httpRequest = _httpContextAccessor.HttpContext.Request;
                var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{file.FileName}{blogImage.FileExtension}";

                blogImage.Url = urlPath;

                await _applicationDbContext.BlogImages.AddAsync(blogImage);
                await _applicationDbContext.SaveChangesAsync();
                return blogImage;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
