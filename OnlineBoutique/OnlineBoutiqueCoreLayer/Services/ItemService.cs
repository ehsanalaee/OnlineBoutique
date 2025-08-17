using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineBoutiqueDataLayer.Context;
using OnlineBoutiqueDataLayer.Entities;

namespace OnlineBoutiqueCoreLayer.Services
{
    public class ItemService : IItemService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ItemService> _logger;

        public ItemService(AppDbContext context, ILogger<ItemService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            try
            {
                return await _context.Items
                    .Include(i => i.Category)
                    .OrderBy(i => i.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all items");
                throw;
            }
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            try
            {
                var item = await _context.Items
                    .Include(i => i.Category)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (item == null)
                {
                    throw new KeyNotFoundException($"Item with ID {id} not found.");
                }

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting item with ID {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Item>> GetItemsByCategoryAsync(int categoryId)
        {
            try
            {
                return await _context.Items
                    .Where(i => i.CategoryId == categoryId)
                    .Include(i => i.Category)
                    .OrderBy(i => i.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting items for category ID {categoryId}");
                throw;
            }
        }

        public async Task<bool> CreateItemAsync(Item item)
        {
            try
            {
                if (item == null)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                // Validate category exists
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == item.CategoryId);
                if (!categoryExists)
                {
                    return false;
                }

                // Validate name uniqueness
                var existingItem = await _context.Items
                    .FirstOrDefaultAsync(i => i.Name == item.Name);

                if (existingItem != null)
                {
                    return false;
                }

                await _context.Items.AddAsync(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new item");
                return false;
            }
        }

        public async Task<bool> UpdateItemAsync(int id, Item item)
        {
            try
            {
                if (item == null)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                var existingItem = await _context.Items.FindAsync(id);

                if (existingItem == null)
                {
                    return false;
                }

                // Validate category exists
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == item.CategoryId);
                if (!categoryExists)
                {
                    return false;
                }

                // Check if name is being changed to an existing name
                if (existingItem.Name != item.Name)
                {
                    var nameExists = await _context.Items
                        .AnyAsync(i => i.Name == item.Name && i.Id != id);

                    if (nameExists)
                    {
                        return false;
                    }
                }

                // Update properties
                existingItem.Name = item.Name;
                existingItem.Description = item.Description;
                existingItem.Price = item.Price;
                existingItem.QuantityInStock = item.QuantityInStock;
                existingItem.Size = item.Size;
                existingItem.Color = item.Color;
                existingItem.ImageUrl = item.ImageUrl;
                existingItem.CategoryId = item.CategoryId;

                _context.Items.Update(existingItem);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating item with ID {id}");
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                var item = await _context.Items.FindAsync(id);

                if (item == null)
                {
                    return false;
                }

                _context.Items.Remove(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting item with ID {id}");
                return false;
            }
        }

        public async Task<bool> UpdateItemStockAsync(int id, int quantityChange)
        {
            try
            {
                var item = await _context.Items.FindAsync(id);

                if (item == null)
                {
                    return false;
                }

                var newQuantity = item.QuantityInStock + quantityChange;
                if (newQuantity < 0)
                {
                    return false; // Prevent negative stock
                }

                item.QuantityInStock = newQuantity;
                _context.Items.Update(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating stock for item with ID {id}");
                return false;
            }
        }
    }
}
