using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NaciboNotesPlatform.Controllers
{
    public class CategoryController : Controller
    {
        //CategoryRep _categoryRep;
        //public CategoryController()
        //{
        //    _categoryRep = new CategoryRep();
        //}
        //public ActionResult Select(int? id)
        //{
        //    if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        //    Category c = _categoryRep.Find(id.Value);

        //    if (c == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    TempData["categoryNotes"] = c.Notes;
        //    return RedirectToAction("Index", "Home");
        //}
    }
}