using OnlineBoutiqueDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBoutiqueCoreLayer.Services
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(int id);
        Task<IEnumerable<Item>> GetItemsByCategoryAsync(int categoryId);
        Task<bool> CreateItemAsync(Item item);
        Task<bool> UpdateItemAsync(int id, Item item);
        Task<bool> DeleteItemAsync(int id);
        Task<bool> UpdateItemStockAsync(int id, int quantityChange);
    }
}
