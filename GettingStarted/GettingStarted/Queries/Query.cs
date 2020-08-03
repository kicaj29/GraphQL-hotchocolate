using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GettingStarted.Queries
{
    public class Query
    {
        public string Hello => "World";
        public string CurrentDateTime => DateTime.Now.ToString();
        public string HelloWithParam(string name) => $"Greetings {name}!";
    }
}
