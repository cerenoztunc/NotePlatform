using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaciboNotesPlatform.ViewModels
{
    public class NoteVM
    {
        public List<Note> Notes { get; set; }
        public Note MostLikedNote { get; set; }

    }
}