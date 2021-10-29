using NaciboNotesPlatform.Filters;
using NaciboNotesPlatform.Models;
using Project.BLL.Managers;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NaciboNotesPlatform.Controllers
{
    [Exc]
    public class CommentController : Controller
    {
        NoteManager noteManager = new NoteManager();
        CommentManager commentManager = new CommentManager();
        public ActionResult ShowNoteComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Note note = noteManager.FirstOrDefault(x => x.ID == id);

            if (note == null)
            {
                return HttpNotFound();
            }
            

            return PartialView("_PartialComments",note.Comments.Where(x=>x.Status != Project.ENTITIES.Enums.DataStatus.Deleted).ToList());
        }

        [Auth]
        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Comment comment = commentManager.FirstOrDefault(x => x.ID == id);
            if(comment == null)
            {
                return new HttpNotFoundResult();
            }
            comment.Text = text;
            if( commentManager.Update(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Comment comment = commentManager.FirstOrDefault(x => x.ID == id);
            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
           
            if (commentManager.Delete(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [Auth]
        [HttpPost]
        public ActionResult Create(Comment comment, int? noteid)
        {
            if (ModelState.IsValid)
            {
                if (noteid == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                Note note = noteManager.FirstOrDefault(x => x.ID == noteid);
                if (note == null)
                {
                    return new HttpNotFoundResult();
                }

                comment.Note = note;
                comment.NaciboUser = CurrentSession.User;

                if (commentManager.Add(comment) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
               
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

    }
}