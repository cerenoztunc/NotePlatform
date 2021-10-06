
using Project.BLL;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.BLL.DesignPatterns.SingletonPattern;
using Project.BLL.UserManager;
using Project.ENTITIES.Messages;
using Project.ENTITIES.Models;
using Project.ENTITIES.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace NaciboNotesPlatform.Controllers
{
    public class HomeController : Controller
    {
        NoteRep _noteRep;
        CategoryRep _categoryRep;
        NaciboUserRep _nUserRep;

        

        public HomeController()
        {
            _noteRep = new NoteRep();
            _categoryRep = new CategoryRep();
            _nUserRep = new NaciboUserRep();
        }
        public ActionResult Index()
        {
            //if (TempData["categoryNotes"] != null)
            //{
            //    return View(TempData["categoryNotes"] as List<Note>);
            //}

            return View(_noteRep.GetAll().OrderByDescending(x=>x.CreatedDate).ToList());
        }
        public ActionResult ByCategory(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            Category c = _categoryRep.Find(id.Value);

            if (c == null)
            {
                return HttpNotFound();
            }


            return View("Index", c.Notes.OrderByDescending(x => x.CreatedDate).ToList());
        }
        public ActionResult MostLiked()
        {

            return View("Index", _noteRep.GetAll().OrderByDescending(x=>x.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                NaciboUserManager num = new NaciboUserManager();
                BussinessLayerResult<NaciboUser> res = num.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    
                    if(res.Errors.Where(x=>x.Code == ErrorMessages.UserIsNotActive).FirstOrDefault() != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/2345678";
                    }
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                Session["login"] = res.Result;

                return RedirectToAction("Index");
            }
            
            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                NaciboUserManager num = new NaciboUserManager();
                BussinessLayerResult<NaciboUser> res = num.RegisterUser(model);

                if(res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                return RedirectToAction("RegisterOk");

            }
            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid activate_id)
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}