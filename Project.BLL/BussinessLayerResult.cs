using Project.ENTITIES.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL
{
    public class BussinessLayerResult<T> where T:class
    {
        public BussinessLayerResult()
        {
            Errors = new List<ErrorMessageObj>();
        }
        public List<ErrorMessageObj> Errors { get; set; }
        public T Result { get; set; }

        public void AddError(ErrorMessages code, string message)
        {
            Errors.Add(new ErrorMessageObj() {
                
                Code = code,
                Message = message
            
            });
        }
    }
}
