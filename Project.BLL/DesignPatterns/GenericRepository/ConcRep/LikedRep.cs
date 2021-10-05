using Project.BLL.DesignPatterns.GenericRepository.BaseRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesignPatterns.GenericRepository.ConcRep
{
    public class LikedRep:BaseRepository<Liked>
    {
        //public LikedRep()
        //{
        //    List<NaciboUser> userList = _db.NaciboUsers.ToList();
        //    for (int i = 0; i < FakeData.NumberData.GetNumber(1,3); i++)
        //    {
        //        Liked liked = new Liked
        //        {
        //            LikedUserID = FakeData.NumberData.GetNumber(2,4),
        //            NoteID = FakeData.NumberData.GetNumber(8,14),
        //            LikedUser = userList[i]
        //        };
        //        Add(liked);
        //    }
        //}
    }
}
