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
            Errors = new List<string>();
        }
        public List<string> Errors { get; set; }
        public T Result { get; set; }
    }
}
