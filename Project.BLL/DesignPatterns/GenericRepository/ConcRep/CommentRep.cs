using Project.DLL.DesignPatterns.GenericRepository.BaseRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.DesignPatterns.GenericRepository.ConcRep
{
    public class CommentRep : BaseRepository<Comment>
    {
        //public CommentRep()
        //{
        //    for (int i = 0; i < FakeData.NumberData.GetNumber(3, 5); i++)
        //    {
        //        Comment comment = new Comment
        //        {
                    
        //            Text = FakeData.TextData.GetSentence(),
        //            UserID = FakeData.NumberData.GetNumber(2, 4),
        //            NoteID = FakeData.NumberData.GetNumber(8, 14),

        //        };
        //        Add(comment);
        //    }
        //}

    }
}

