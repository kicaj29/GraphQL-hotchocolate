using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.Mutations
{
    public class CreateBookInput
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
    }

    public class DeleteBookInput
    {
        public int Id { get; set; }
    }
}
