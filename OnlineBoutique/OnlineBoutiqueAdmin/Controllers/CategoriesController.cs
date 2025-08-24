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
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _service;

        public CategoriesController(CategoryService service)
        {
            _service = service;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = _service.GetAllCategoriesAndChildrenAsync();
            return View(await categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _service.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public async Task<IActionResult> Create()
        {

            var categories = await _service.GetAllCategoriesAsync();
            var categoryList = new List<Category>
                {
                    new Category { Id = 0 , Name = "-- None --" }
                };
            categoryList.AddRange(categories);
            ViewData["ParentId"] = new SelectList(categoryList, "Id", "Name");


            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Slug,Description,ParentId")] Category category)
        {
            if (category.ParentId == 0)
            {
                category.ParentId = null;
            }
            await _service.CreateCategoryAsync(category);
            return RedirectToAction(nameof(Index));
            if (ModelState.IsValid)
            {
                
            }
            ViewData["ParentId"] = new SelectList(await _service.GetAllCategoriesAsync(), "Id", "Description", category.ParentId);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _service.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            var categories = await _service.GetAllCategoriesAsync();
            var categoryList = new List<Category>
                {
                    new Category { Id = 0, Name = "-- None --" }
                };
            categoryList.AddRange(categories);
            ViewData["ParentId"] = new SelectList(categoryList, "Id", "Name");
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Slug,Description,ParentId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            if (category.ParentId == 0)
            {
                category.ParentId = null;
            }
            try
            {
                await _service.UpdateCategoryAsync(id, category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            if (ModelState.IsValid)
            {
                
            }
            var categories = await _service.GetAllCategoriesAsync();
            var categoryList = new List<Category>
                {
                    new Category { Id = 0, Name = "-- None --" }
                };
            categoryList.AddRange(categories);
            ViewData["ParentId"] = new SelectList(categoryList, "Id", "Name");
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _service.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var category = await _service.GetCategoryByIdAsync(id);
            if (category != null)
            {
                await _service.DeleteCategoryAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            var category = _service.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return false;
            }
            else {
                return true;
            }


        }
    }
}
