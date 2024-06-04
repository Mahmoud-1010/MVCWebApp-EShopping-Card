using AutoMapper;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        public IUnitOfWork _UnitOfWork { get; }
        public IMapper Mapper { get; }

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            Mapper = mapper;
        }
        // GET: CategoriesController
        // all list and searched list
        public async Task<ActionResult> Index(string SearchValue = "")
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                IEnumerable<Category> categories = await _UnitOfWork.CategoryRepository.GetAllAsync();
                List<CategoryViewModel> categoriesVM = Mapper.Map<IEnumerable<Category>, List<CategoryViewModel>>(categories);
                return View(categoriesVM);
            }
            else
            {
                var categories = await _UnitOfWork.CategoryRepository.Search(SearchValue);
                var MappedCategories = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(categories);
                return View(MappedCategories);
            }
        }

        // GET: CategoriesController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryViewModel cateegoryVM)
        {
            if (ModelState.IsValid)
            {
                var MapedCategory = Mapper.Map<CategoryViewModel, Category>(cateegoryVM);
                await _UnitOfWork.CategoryRepository.InsertAsync(MapedCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(cateegoryVM);
        }

        // GET: CategoriesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id == null)
                return NotFound();
            Category category = await _UnitOfWork.CategoryRepository.GetByIdAsync(p => p.Id == id);
            CategoryViewModel categoryVM = Mapper.Map<Category, CategoryViewModel>(category);
            return View(categoryVM);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryViewModel categoryVM)
        {
            //if(id!=categoryVM.Id)
            //    return NotFound();
            if (ModelState.IsValid)
            {

                Category MapedCategory = Mapper.Map<CategoryViewModel, Category>(categoryVM);

                await _UnitOfWork.CategoryRepository.Update(MapedCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(categoryVM);
        }

        // GET: CategoriesController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var Category = await _UnitOfWork.CategoryRepository.GetByIdAsync(p=>p.Id==id);
            if (Category == null)
                return NotFound();
            var MappedCategory = Mapper.Map<Category, CategoryViewModel>(Category);

            return View(MappedCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CategoryViewModel categoryVM)
        {
            if (id != categoryVM.Id)
                return NotFound();
            try
            {

                var MappedCategoery = Mapper.Map<CategoryViewModel, Category>(categoryVM);
                
                await _UnitOfWork.CategoryRepository.DeleteAsync(MappedCategoery);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(categoryVM);
            }
        }

    }
}
