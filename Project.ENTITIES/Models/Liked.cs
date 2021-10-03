using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Liked:BaseEntity
    {
        public int NoteID { get; set; }
        public int LikedUserID { get; set; }

        //Relational properties
        public virtual Note Note { get; set; }
        public virtual NaciboUser LikedUser { get; set; }
    }
}
