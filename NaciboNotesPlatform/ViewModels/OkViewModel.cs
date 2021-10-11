using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaciboNotesPlatform.ViewModels
{
    public class OkViewModel:NotifyViewModelBase<string>
    {
        public OkViewModel()
        {
            Title = "işlem Başarılı";
        }
    }
}