using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Comment:BaseEntity
    {
        
        public string Text { get; set; }
        public int UserID { get; set; }
        public int NoteID { get; set; }
        //Relational properties

        [ForeignKey("UserID")]
        public virtual NaciboUser NaciboUser { get; set; }

        [ForeignKey("NoteID")]
        public virtual Note Note { get; set; }
    }
}
