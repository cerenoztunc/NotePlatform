using NaciboNotesPlatform.Models;
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
            NaciboUser user = CurrentSession.User;
            if (user != null) return user.UserName;
            else return "system";
        }
    }
}