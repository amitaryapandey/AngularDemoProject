using CodePulse.API.Data;
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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        //
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryRequestDto createCategoryRequestDto)
        {
            var category = new Category()
            {
                Name = createCategoryRequestDto.Name,
                UrlHandle = createCategoryRequestDto.UrlHandle
            };
            var createCategory = await _categoryRepository.CreateCategory(category);
            var categoryDto = new CategoryDto()
            {
                Id = createCategory.Id,
                Name = createCategory.Name,
                UrlHandle = createCategory.UrlHandle
            };

            return Ok(categoryDto);
        }
        //GET: /api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            var response = new List<CategoryDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDto() { Id = category.Id, Name = category.Name, UrlHandle = category.UrlHandle });
            }
            return Ok(response);
        }

        //GET: /api/categories/id
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);
            if (existingCategory == null) { return NotFound(); }
            var existingCategoryResult = new CategoryDto()
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };
            return Ok(existingCategoryResult);
        }
        //PUT: /api/categories/id
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequest updateCategoryRequest)
        {

            var category = new Category()
            {
                Id = id,
                Name = updateCategoryRequest.Name,
                UrlHandle = updateCategoryRequest.UrlHandle
            };
            var updateCategoryResult = await _categoryRepository.UpdateCategoryAsync(category);
            if (updateCategoryResult == null)
            {
                return NotFound();
            }
            var categoryDto = new CategoryDto()
            {
                Id = updateCategoryResult.Id,
                Name = updateCategoryResult.Name,
                UrlHandle = updateCategoryResult.UrlHandle
            };
            return Ok(categoryDto);
        }
        //Delete :/api/categories/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var deletedCategory = await _categoryRepository.DeleteCategoryAsync(id);
            if (deletedCategory == null) { return NotFound(); }
            var categoryDto = new CategoryDto() 
            { 
                Id = deletedCategory.Id, 
                Name = deletedCategory.Name, 
                UrlHandle = deletedCategory.UrlHandle 
            };

            return Ok(categoryDto);
        }

    }
}
