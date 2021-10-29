
using NaciboNotesPlatform.Filters;
using NaciboNotesPlatform.Models;
using NaciboNotesPlatform.ViewModels;
using Project.BLL;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.BLL.DesignPatterns.SingletonPattern;
using Project.BLL.Managers;
using Project.BLL.Results;
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
    [Exc]
    public class HomeController : Controller
    {
        NoteRep _noteRep;
        CategoryRep _categoryRep;
        NaciboUserRep _nUserRep;
        NaciboUserManager num;
        public HomeController()
        {
            _noteRep = new NoteRep();
            _categoryRep = new CategoryRep();
            _nUserRep = new NaciboUserRep();
            num = new NaciboUserManager();
        }
        public ActionResult Index()
        {
            //throw new Exception("Herhangi bir hata"); //HasError sayfasını görmek için deneme

            //if (TempData["categoryNotes"] != null)
            //{
            //    return View(TempData["categoryNotes"] as List<Note>);
            //}
            var notes = _noteRep.GetActives().Where(x => x.IsDraft == false).OrderByDescending(x => x.CreatedDate).ToList();
            int mostLikedCount = notes.Max(x => x.LikeCount);
            var mostLikedNote = notes.FirstOrDefault(x => x.LikeCount == mostLikedCount);

            NoteVM viewModel = new NoteVM()
            {
                MostLikedNote = mostLikedNote,
                Notes = notes
            };
            

            return View(viewModel);
        }
        public ActionResult ByCategory(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
           
            List<Note> notes = _noteRep.ListQuaryable().Where(x => x.IsDraft == false && x.CategoryID == id).OrderByDescending(x => x.CreatedDate).ToList();
            return View("Index", notes);
        }
        public ActionResult MostLiked()
        {

            return View("Index", _noteRep.GetActives().OrderByDescending(x => x.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {
            BussinessLayerResult<NaciboUser> res = num.GetUserById(CurrentSession.User.ID);
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

        [Auth]
        public ActionResult ShowNotes(NaciboUser naciboUser)
        {
            NaciboUser currentUser = Session["login"] as NaciboUser;
            if(naciboUser.Notes != null)
            {
                currentUser.Notes = _noteRep.Where(x => x.NaciboUser.Notes == naciboUser.Notes);
            }
            else if(naciboUser == null)
            {
                ViewBag.NoNote = "Not bulunmamaktadır.";
            }
            
            return View(currentUser);
        }

        [Auth]
        public ActionResult EditProfile()
        {
            BussinessLayerResult<NaciboUser> res = num.GetUserById(CurrentSession.User.ID);
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

        [Auth]
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

                CurrentSession.Set<NaciboUser>("login", res.Result); //Profil güncellendiği için session güncellenmeli

                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }

        [Auth]
        public ActionResult DeleteProfile()
        {
            
            BussinessLayerResult<NaciboUser> res = num.DeleteUserByID(CurrentSession.User.ID);

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

                CurrentSession.Set<NaciboUser>("login", res.Result); //Session'a kullanıcı bilgisi saklama

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
        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult HasError()
        {
            return View();
        }
    }
}