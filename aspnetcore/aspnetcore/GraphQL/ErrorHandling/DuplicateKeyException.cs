using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.ErrorHandling
{
    public class DuplicateKeyException : Exception
    {
        public int Id { get; private set; }
        public string Type { get; private set; }

        public DuplicateKeyException(int id, Type type, string message):base(message)
        {
            this.Id = id;
            this.Type = type.Name;
        }
    }
}
