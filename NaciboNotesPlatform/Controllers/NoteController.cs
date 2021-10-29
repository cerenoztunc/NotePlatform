using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NaciboNotesPlatform.Data;
using NaciboNotesPlatform.Filters;
using NaciboNotesPlatform.Models;
using Project.BLL.Managers;
using Project.ENTITIES.Models;

namespace NaciboNotesPlatform.Controllers
{
    [Exc]
    public class NoteController : Controller
    {
        NoteManager noteManager = new NoteManager();
        CategoryManager categoryManager = new CategoryManager();
        LikedManager likedManager = new LikedManager();

        [Auth]
        public ActionResult Index()
        {
            var notes = noteManager.ListQuaryable()
                .Where(x => x.NaciboUser.ID == CurrentSession.User.ID && x.Status != Project.ENTITIES.Enums.DataStatus.Deleted )
                .Include("Category").Include("NaciboUser")
                .OrderByDescending(x => x.CreatedDate);
          
            return View(notes.ToList());
        }

        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        [Auth]
        public ActionResult MyLikedNotes()
        {
            var likednotes = likedManager.ListQuaryable()
                .Include("LikedUser")
                .Include("Note")
                .Where(x => x.LikedUser.ID == CurrentSession.User.ID)
                .Select(x => x.Note)
                .Include("Category")
                .Include("NaciboUser")
                .OrderByDescending(x => x.CreatedDate);
 
            return View("Index", likednotes.ToList());
        }

        [Auth]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(CacheHelper.GetCategoriesFromCache(), "ID", "Title");
            return View();
        }
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            if (ModelState.IsValid)
            {
                note.UserID = CurrentSession.User.ID;
                noteManager.Add(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(CacheHelper.GetCategoriesFromCache(), "ID", "Title", note.CategoryID);
            return View(note);
        }
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(CacheHelper.GetCategoriesFromCache(), "ID", "Title", note.CategoryID);
            return View(note);
        }
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            if (ModelState.IsValid)
            {
                Note n = noteManager.FirstOrDefault(x => x.ID == note.ID);
                n.IsDraft = note.IsDraft;
                n.CategoryID = note.CategoryID;
                n.Text = note.Text;
                n.Title = note.Title;

                noteManager.Update(n);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(CacheHelper.GetCategoriesFromCache(), "ID", "Title", note.CategoryID);
            return View(note);
        }
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }
        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = noteManager.Find(id);
            noteManager.Delete(note);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            if (CurrentSession.User != null)
            {
                int userid = CurrentSession.User.ID;
                List<int> likedNoteIds = new List<int>();

                if (ids != null)
                {
                    likedNoteIds = likedManager.List(
                        x => x.LikedUser.ID == userid && ids.Contains(x.Note.ID)).Select(
                        x => x.Note.ID).ToList();
                }
                else
                {
                    likedNoteIds = likedManager.List(
                        x => x.LikedUser.ID == userid).Select(
                        x => x.Note.ID).ToList();
                }

                return Json(new { result = likedNoteIds });
            }
            else
            {
                return Json(new { result = new List<int>() });
            }
        }
        [HttpPost]
        public ActionResult SetLikeState(int noteid, bool liked)
        {
            int res = 0;

            if (CurrentSession.User == null)
                return Json(new { hasError = true, errorMessage = "Beğenme işlemi için giriş yapmalısınız.", result = 0 });

            Liked like =
                likedManager.FirstOrDefault(x => x.Note.ID == noteid && x.LikedUser.ID == CurrentSession.User.ID);

            Note note = noteManager.FirstOrDefault(x => x.ID == noteid);

            if (like != null && liked == false)
            {
                res = likedManager.Delete(like);
            }
            else if (like == null && liked == true)
            {
                res = likedManager.Add(new Liked()
                {
                    LikedUser = CurrentSession.User,
                    Note = note
                });
            }

            if (res > 0)
            {
                if (liked)
                {
                    note.LikeCount++;
                }
                else
                {
                    note.LikeCount--;
                }

                res = noteManager.Update(note);

                return Json(new { hasError = false, errorMessage = string.Empty, result = note.LikeCount });
            }

            return Json(new { hasError = true, errorMessage = "Beğenme işlemi gerçekleştirilemedi.", result = note.LikeCount });
        }
        public ActionResult GetNoteText(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Note note = noteManager.Find(id.Value);

            if (note == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialNoteText", note);
        }

    }
}
