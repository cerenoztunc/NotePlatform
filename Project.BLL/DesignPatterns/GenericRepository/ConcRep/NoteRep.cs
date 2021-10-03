using Project.DLL.DesignPatterns.GenericRepository.BaseRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.DesignPatterns.GenericRepository.ConcRep
{
    public class NoteRep:BaseRepository<Note>
    {
        public NoteRep()
        {
            for (int i = 0; i < FakeData.NumberData.GetNumber(5,9); i++)
            {
                Note note = new Note
                {
                    Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5,25)),
                    Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1,3)),
                    IsDraft = false,
                    LikeCount = FakeData.NumberData.GetNumber(10,50),
                    UserID = FakeData.NumberData.GetNumber(2,4)
                    

                };
                Add(note);
            }
        }
    }
}
