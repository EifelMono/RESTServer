using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTLib
{
    public class RESTHello
    {
        public string Text { get; set; }
        public int Id { get; set; }
    }

    public class HelloResponse : RESTHello
    {
    }

    public class HelloRequest : RESTHello
    {

    }

}
