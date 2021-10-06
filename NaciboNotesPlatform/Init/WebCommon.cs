using Project.COMMON;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaciboNotesPlatform.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            if(HttpContext.Current.Session["login"] != null)
            {
                NaciboUser user = HttpContext.Current.Session["login"] as NaciboUser;
                return user.UserName;
            }
            return null;
        }
    }
}