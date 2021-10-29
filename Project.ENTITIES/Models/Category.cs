using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Category: BaseEntity
    {
        [DisplayName("Kategori Adı")]
        public string Title { get; set; }
        [DisplayName("Açıklama")]
        public string Description { get; set; }

        //Relational properties
        public virtual List<Note> Notes { get; set; }
    }
}
