using HotChocolate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Introduction.SchemaFirst
{
    public class Query
    {
        public string Hello() => "World!(schema first)";

        public string Greetings() => "Greetings!";
    }

    public class QueryResolvers
    {
        //This method must have exactly the same name as function in class Query
        public string Greetings([Parent]Query query)
        {
            var g = query.Greetings();
            return $"Wrapper - {g}" ;
        }
    }

}
