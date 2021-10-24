using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.BLL.Managers;
using Project.ENTITIES.Models;

namespace NaciboNotesPlatform.Controllers
{
    public class CategoryController : Controller
    {
        CategoryManager _categoryManager;
        public CategoryController()
        {
            _categoryManager = new CategoryManager();
        }
        public ActionResult Index()
        {
            return View(_categoryManager.GetActives());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryManager.FirstOrDefault(x=>x.ID == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryManager.Add(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryManager.FirstOrDefault(x => x.ID == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryManager.Update(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryManager.FirstOrDefault(x => x.ID == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = _categoryManager.FirstOrDefault(x => x.ID == id);
            _categoryManager.Delete(category);
            return RedirectToAction("Index");
        }

        
    }
}
