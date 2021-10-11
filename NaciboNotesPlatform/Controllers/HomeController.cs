
using NaciboNotesPlatform.ViewModels;
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

            return View(_noteRep.GetAll().OrderByDescending(x => x.CreatedDate).ToList());
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

            return View("Index", _noteRep.GetAll().OrderByDescending(x => x.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult ShowProfile()
        {
            NaciboUser currentUser = Session["login"] as NaciboUser;
            NaciboUserManager num = new NaciboUserManager();
            BussinessLayerResult<NaciboUser> res = num.GetUserById(currentUser.ID);
            if (res.Errors.Count > 0)
            {
                ErrrorViewModel error = new ErrrorViewModel
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", error);
            }
            return View(res.Result);
        }
        public ActionResult EditProfile()
        {
            NaciboUser currentUser = Session["login"] as NaciboUser;
            NaciboUserManager num = new NaciboUserManager();
            BussinessLayerResult<NaciboUser> res = num.GetUserById(currentUser.ID);
            if (res.Errors.Count > 0)
            {
                ErrrorViewModel error = new ErrrorViewModel
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", error);
            }
            return View(res.Result);

        }
        [HttpPost]
        public ActionResult EditProfile(NaciboUser model, HttpPostedFileBase ProfileImage)
        {
           
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.ID}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    model.ProfileImageFileName = filename;
                }

                NaciboUserManager num = new NaciboUserManager();
                BussinessLayerResult<NaciboUser> res = num.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErrrorViewModel messages = new ErrrorViewModel
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    return View("Error", messages);
                }

                Session["login"] = res.Result;

                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }
        public ActionResult DeleteProfile()
        {
            NaciboUser currentUser = Session["login"] as NaciboUser;
            NaciboUserManager num = new NaciboUserManager();
            BussinessLayerResult<NaciboUser> res = num.DeleteUserByID(currentUser.ID);

            if (res.Errors.Count > 0)
            {
                ErrrorViewModel messages = new ErrrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil silinemedi",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", messages);
            }
            Session.Clear();

            return RedirectToAction("Login");
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

                    if (res.Errors.Where(x => x.Code == ErrorMessages.UserIsNotActive).FirstOrDefault() != null)
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

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                OkViewModel ok = new OkViewModel
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",

                };
                ok.Items.Add(" Lütfen e-posta adresinize gönderilen aktivasyon link'ine tıklayarak hesabınızı aktive ediniz.Hesabınızı aktive etmeden not ekleyemez ve beğenme yapamazsınız.");

                return View("Ok", ok);

            }
            return View(model);
        }
        public ActionResult UserActivate(Guid id)
        {
            NaciboUserManager num = new NaciboUserManager();
            BussinessLayerResult<NaciboUser> res = num.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrrorViewModel error = new ErrrorViewModel
                {
                    Title = "Geçersiz işlem",
                    Items = res.Errors
                };

                return View("Error", error);
            }
            OkViewModel ok = new OkViewModel
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/Login"
            };
            ok.Items.Add("Hesabınız aktifleştirildi. Artık not ekleyebilir ve beğenme yapabilirsiniz.");
            return RedirectToAction("Ok", ok);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}