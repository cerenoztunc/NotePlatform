using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers
{
    public class CategoryManager : BaseManager<Category>
    {
        NoteManager noteManager = new NoteManager();
        LikedManager likedManager = new LikedManager();
        CommentManager commentManager = new CommentManager();
        public override int Delete(Category item)
        {
            //Kategori ile iliskili note'ların silinmesi
            foreach (Note note in item.Notes.ToList())
            {
                //Note ile ilgili liked'ların silinmesi
                foreach (Liked liked in note.Likeds.ToList())
                {
                    likedManager.Delete(liked);
                }
                //Note ile iliskili comment'lerin silinmesi
                foreach (Comment comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }

                noteManager.Delete(note);
            }

            return base.Delete(item);
        }

    }
}
