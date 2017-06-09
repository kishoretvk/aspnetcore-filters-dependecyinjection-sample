using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.service
{
    public class Impl : ISimpleRepo
    {
        public samplemodel complexObj(string samplestring)
        {
            var obj = new samplemodel()
            {
                email = "abc@gmail.com",
                
                name = "sampledata",
                number = 10
            };
            return obj;
        }

        public string samplemethod(string hello)
        {
            return "welcome to usa";
        }
    }
}
