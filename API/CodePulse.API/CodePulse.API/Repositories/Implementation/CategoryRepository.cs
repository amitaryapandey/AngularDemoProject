using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Category> CreateCategory(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> DeleteCategoryAsync(Guid id)
        {
            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(existingCategory is null)
            {
                return null;
            }
             _dbContext.Categories.Remove(existingCategory);
            await _dbContext.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
             return await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if(existingCategory!=null)
            {
                _dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await _dbContext.SaveChangesAsync();
                return category;
            }
            return null;
        }

        async Task<IEnumerable<Category>> ICategoryRepository.GetAllCategoriesAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }
    }
}
