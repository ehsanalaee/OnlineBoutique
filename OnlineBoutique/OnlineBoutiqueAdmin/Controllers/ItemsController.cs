using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBoutiqueCoreLayer.Services;
using OnlineBoutiqueDataLayer.Context;
using OnlineBoutiqueDataLayer.Entities;

namespace OnlineBoutiqueAdmin.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemService _service;
        private readonly ICategoryService _categoryService;


        public ItemsController(ItemService service, CategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }


        // GET: Items
        public async Task<IActionResult> Index()
        {
            var appDbContext = _service.GetAllItemsAsync();
            return View(await appDbContext);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _service.GetItemByIdAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAndChildrenAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,QuantityInStock,Size,Color,ImageUrl,CategoryId")] Item item)
        {
            //if (ModelState.IsValid)
            //{
                
            //}
            await _service.CreateItemAsync(item);
            return RedirectToAction(nameof(Index));
            //var categories = await _categoryService.GetAllCategoriesAndChildrenAsync();
            //ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", item.CategoryId);
            //return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _service.GetItemByIdAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }
            var categories = await _categoryService.GetAllCategoriesAndChildrenAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", item.CategoryId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,QuantityInStock,Size,Color,ImageUrl,CategoryId")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                
            //}
            try
            {
                await _service.UpdateItemAsync(id, item);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(item.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //var categories = await _categoryService.GetAllCategoriesAndChildrenAsync();
            //ViewData["CategoryId"] = new SelectList(categories , "Id", "Name", item.CategoryId);
            //return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _service.GetItemByIdAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _service.GetItemByIdAsync(id);

            if (item != null)
            {
                await _service.DeleteItemAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            var item = _service.GetItemByIdAsync(id);
            if (item == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
