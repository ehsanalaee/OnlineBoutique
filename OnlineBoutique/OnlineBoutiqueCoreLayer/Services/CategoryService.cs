using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineBoutiqueDataLayer.Context;
using OnlineBoutiqueDataLayer.Entities;

namespace OnlineBoutiqueCoreLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(AppDbContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _context.Categories
                    .Include(c => c.Children)
                    .Where(c => c.ParentId == null)
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all categories");
                throw;
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID {id} not found.");
                }

                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting category with ID {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetChildCategoriesAsync(int parentId)
        {
            try
            {
                return await _context.Categories
                    .Where(c => c.ParentId == parentId)
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting children of category with ID {parentId}");
                throw;
            }
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            try
            {
                if (category == null)
                {
                    throw new ArgumentNullException(nameof(category));
                }

                // Validate name uniqueness
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == category.Name);

                if (existingCategory != null)
                {
                    return false;
                }

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new category");
                return false;
            }
        }

        public async Task<bool> UpdateCategoryAsync(int id, Category category)
        {
            try
            {
                if (category == null)
                {
                    throw new ArgumentNullException(nameof(category));
                }

                var existingCategory = await _context.Categories.FindAsync(id);

                if (existingCategory == null)
                {
                    return false;
                }

                // Check if name is being changed to an existing name
                if (existingCategory.Name != category.Name)
                {
                    var nameExists = await _context.Categories
                        .AnyAsync(c => c.Name == category.Name && c.Id != id);

                    if (nameExists)
                    {
                        return false;
                    }
                }

                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                existingCategory.ParentId = category.ParentId;

                _context.Categories.Update(existingCategory);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating category with ID {id}");
                return false;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return false;
                }

                // Check if category has children
                if (category.Children.Any())
                {
                    return false;
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting category with ID {id}");
                return false;
            }
        }
    }
}
