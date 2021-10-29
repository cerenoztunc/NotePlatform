using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Note: BaseEntity
    {
        public Note()
        {
            Comments = new List<Comment>();
            Likeds = new List<Liked>();
        }

        [DisplayName("Başlık")]
        public string Title { get; set; }
        [DisplayName("Metin")]
        public string Text { get; set; }
        [DisplayName("Taslak")]
        public bool IsDraft { get; set; }
        [DisplayName("Beğeni")]
        public int LikeCount { get; set; }
        [DisplayName("Kategori")]
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
