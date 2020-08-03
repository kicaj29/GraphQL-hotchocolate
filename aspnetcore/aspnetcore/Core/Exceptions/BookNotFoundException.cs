using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.Core.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public int BookId { get; internal set; }
    }
}
