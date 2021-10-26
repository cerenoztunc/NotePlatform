using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NaciboNotesPlatform.Data;
using NaciboNotesPlatform.Models;
using Project.BLL.Managers;
using Project.ENTITIES.Models;

namespace NaciboNotesPlatform.Controllers
{
    public class NoteController : Controller
    {
        NoteManager noteManager = new NoteManager();
        CategoryManager categoryManager = new CategoryManager();
        LikedManager likedManager = new LikedManager();
        public ActionResult Index()
        {
            var notes = noteManager.ListQuaryable()
                .Where(x => x.NaciboUser.ID == CurrentSession.User.ID && x.Status != Project.ENTITIES.Enums.DataStatus.Deleted )
                .Include("Category").Include("NaciboUser")
                .OrderByDescending(x => x.CreatedDate);
          
            return View(notes.ToList());
        }

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
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(categoryManager.GetActives(), "ID", "Title");
            return View();
        }

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

            ViewBag.CategoryID = new SelectList(categoryManager.GetActives(), "ID", "Title", note.CategoryID);
            return View(note);
        }
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
            ViewBag.CategoryID = new SelectList(categoryManager.GetActives(), "ID", "Title", note.CategoryID);
            return View(note);
        }

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
            ViewBag.CategoryID = new SelectList(categoryManager.GetActives(), "ID", "Title", note.CategoryID);
            return View(note);
        }
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = noteManager.Find(id);
            noteManager.Delete(note);

            return RedirectToAction("Index");
        }

    }
}
