using Project.BLL.DesignPatterns.GenericRepository.BaseRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesignPatterns.GenericRepository.ConcRep
{
    public class NaciboUserRep:BaseRepository<NaciboUser>
    {
        public NaciboUserRep()
        {
            
            
        }

        public int DeleteUser(NaciboUser item)
        {
            item.IsActive = false;
            item.Status = ENTITIES.Enums.DataStatus.Deleted;
            item.DeletedDate = DateTime.Now;
            var result = _db.SaveChanges();
            return result;
        }
        
    }
}

