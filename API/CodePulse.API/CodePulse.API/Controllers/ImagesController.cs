using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;
        public ImagesController(IImageRepository imageRepository) 
        {
            this.imageRepository = imageRepository;
        }
        // GET
        [HttpGet]
        public async Task<IEnumerable<BlogImage>> GetImagesAsync()
        {
            var images = await imageRepository.GetBlogImages();
            var response = new List<BlogImage>();
            foreach (var image in images)
            {
                response.Add(new BlogImage
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Url = image.Url
                });
            }

            return response;
        }
        //Post: {apibaseurl}/api/images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage()
                {
                    FileName = fileName,
                    FileExtension = Path.GetExtension(file.FileName),
                    Title = title,
                    DateCreated = DateTime.Now
                };

                blogImage = await imageRepository.Upload(file, blogImage);
                var response = new BlogImageDto()
                {
                    FileName = blogImage.FileName,
                    Title = blogImage.Title,
                    FileExtension = blogImage.FileExtension,
                    DateCreated = blogImage.DateCreated
                };

                return Ok(response);
            }

            return BadRequest();
        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtension.Contains(Path.GetExtension(file.FileName).ToLower())) 
            {
                ModelState.AddModelError("file", "Unsupport file format");
            }
            if(file.Length> 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }
    }
}
