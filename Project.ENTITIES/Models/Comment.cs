using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Comment:BaseEntity
    {
        
        public string Text { get; set; }
        public int? UserID { get; set; }
        public int? NoteID { get; set; }
        //Relational properties
        public virtual NaciboUser NaciboUser { get; set; }
        public virtual Note Note { get; set; }
    }
}
