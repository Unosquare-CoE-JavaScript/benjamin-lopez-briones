using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AuthorController(ApplicationDbContext a)
        {
            _db = a;
        }

        public IActionResult Index()
        {
            List<Authors> objList = _db.Authors.ToList();
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            Authors obj = new Authors();
            if (id == null)
            {
                return View(obj);
            }
            obj = _db.Authors.FirstOrDefault(u => u.Author_ID == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Authors obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Author_ID == 0)
                {
                    _db.Authors.Add(obj);
                }
                else
                {
                    _db.Authors.Update(obj);
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
            var ObjDB = _db.Authors.FirstOrDefault(u => u.Author_ID == id);
            _db.Authors.Remove(ObjDB);
            _db.SaveChanges();
            return RedirectToActionPermanent(nameof(Index));
        }
    }
}
