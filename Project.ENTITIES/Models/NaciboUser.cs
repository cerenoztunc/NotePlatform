using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class NaciboUser: BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string ProfileImageFileName { get; set; }
        public bool IsActive { get; set; }

        public Guid ActivateGuid { get; set; }
        public bool IsAdmin { get; set; }

        //Relationa properties
        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likeds { get; set; }

    }
}
