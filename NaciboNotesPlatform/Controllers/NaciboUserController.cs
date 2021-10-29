using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NaciboNotesPlatform.Filters;
using Project.BLL.Managers;
using Project.BLL.Results;
using Project.ENTITIES.Models;

namespace NaciboNotesPlatform.Controllers
{
    [Exc]
    [Auth]
    [AuthAdmin]
    public class NaciboUserController : Controller
    {
        private NaciboUserManager naciboUserManager = new NaciboUserManager();
        public ActionResult Index()
        {
            return View(naciboUserManager.GetActives());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NaciboUser naciboUser = naciboUserManager.FirstOrDefault(x=>x.ID == id.Value);
            if (naciboUser == null)
            {
                return HttpNotFound();
            }
            return View(naciboUser);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NaciboUser naciboUser)
        {
            if (ModelState.IsValid)
            {
                BussinessLayerResult<NaciboUser> res = naciboUserManager.Add(naciboUser);
                if(res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(naciboUser);
                }
                return RedirectToAction("Index");
            }
            return View(naciboUser);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NaciboUser naciboUser = naciboUserManager.FirstOrDefault(x => x.ID == id.Value);
            if (naciboUser == null)
            {
                return HttpNotFound();
            }
            return View(naciboUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NaciboUser naciboUser)
        {
            if (ModelState.IsValid)
            {
                BussinessLayerResult<NaciboUser> res = naciboUserManager.Update(naciboUser);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(naciboUser);
                }
                return RedirectToAction("Index");
            }
            return View(naciboUser);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NaciboUser naciboUser = naciboUserManager.FirstOrDefault(x => x.ID == id.Value);
            if (naciboUser == null)
            {
                return HttpNotFound();
            }
            return View(naciboUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NaciboUser naciboUser = naciboUserManager.FirstOrDefault(x => x.ID == id);
            naciboUserManager.Delete(naciboUser);
            return RedirectToAction("Index");
        }

    }
}
