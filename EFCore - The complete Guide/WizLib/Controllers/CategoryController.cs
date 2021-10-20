using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace WizLib.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext a)
        {
            _db = a;
        }

        public IActionResult Index()
        {
            List<Category> objList = _db.Categories.AsNoTracking().ToList();
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            Category obj = new Category();
            if(id == null)
            {
                return View(obj);
            }

            obj = _db.Categories.FirstOrDefault(u => u.Category_ID == id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Category_ID == 0)
                {
                    //create
                    _db.Categories.Add(obj);
                }
                else
                {
                    //update
                    _db.Categories.Update(obj);
                }

                _db.SaveChanges();
                return RedirectToActionPermanent(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        public IActionResult Delete(int id)
        {
            var objFromDB = _db.Categories.FirstOrDefault(u => u.Category_ID == id);
            _db.Categories.Remove(objFromDB);
            _db.SaveChanges();
            return RedirectToActionPermanent(nameof(Index));
        }

        public IActionResult CreateMultiple2()
        {
            List<Category> categories = new List<Category>();
            for(int i = 1; i <= 2; i++)
            {
                categories.Add(new Category { Name = Guid.NewGuid().ToString() });
                //_db.Categories.Add(new Category { Name = Guid.NewGuid().ToString() });
            }
            _db.Categories.AddRange(categories);
            _db.SaveChanges();
            return RedirectToActionPermanent(nameof(Index));
        }

        public IActionResult CreateMultiple5()
        {
            for (int i = 1; i <= 5; i++)
            {
                _db.Categories.Add(new Category { Name = Guid.NewGuid().ToString() });
            }
            _db.SaveChanges();
            return RedirectToActionPermanent(nameof(Index));
        }

        public IActionResult RemoveMultiple2()
        {
            List<Category> categories = _db.Categories.OrderByDescending(u => u.Category_ID).Take(2).ToList();
            _db.Categories.RemoveRange(categories);
            _db.SaveChanges();
            return RedirectToActionPermanent(nameof(Index));
        }
        public IActionResult RemoveMultiple5()
        {
            List<Category> categories = _db.Categories.OrderByDescending(u => u.Category_ID).Take(5).ToList();
            _db.Categories.RemoveRange(categories);
            _db.SaveChanges();
            return RedirectToActionPermanent(nameof(Index));
        }
    }
}
