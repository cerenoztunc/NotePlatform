using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Note: BaseEntity
    {
       
        public string Title { get; set; }

        public string Text { get; set; }
        public bool IsDraft { get; set; }
        public int LikeCount { get; set; }
        public int? CategoryID { get; set; }
        public int? UserID { get; set; }



        //Relational properties
        public virtual Category Category { get; set; }
        [ForeignKey("UserID")]
        public virtual NaciboUser NaciboUser { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likeds { get; set; }

    }
}
