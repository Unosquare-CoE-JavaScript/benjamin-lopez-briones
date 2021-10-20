using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class PublisherController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PublisherController(ApplicationDbContext a)
        {
            _db = a;
        }
        public IActionResult Index()
        {
            List<Publisher> objList = _db.Publishers.ToList();
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            Publisher obj = new Publisher();
            if(id == null)
            {
                return View(obj);
            }
            obj = _db.Publishers.FirstOrDefault(u => u.PublisherID == id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Publisher obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.PublisherID == 0) {
                    _db.Publishers.Add(obj);
                }
                else
                {
                    _db.Publishers.Update(obj);
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
            var ObjDB = _db.Publishers.FirstOrDefault(u => u.PublisherID == id);
            _db.Publishers.Remove(ObjDB);
            _db.SaveChanges();
            return RedirectToActionPermanent(nameof(Index));
        }
    }
}
