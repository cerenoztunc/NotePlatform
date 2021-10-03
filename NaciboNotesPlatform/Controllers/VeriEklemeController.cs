using Project.DLL.DesignPatterns.GenericRepository.ConcRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NaciboNotesPlatform.Controllers
{
    public class VeriEklemeController : Controller
    {
        // GET: VeriEkleme
       
        
        public VeriEklemeController()
        {
           
           
        }
        public ActionResult AddNaciboUser()
        {
           
            return View();
        }
    }
}