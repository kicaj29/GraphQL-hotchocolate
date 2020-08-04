using aspnetcore.GraphQL;
using aspnetcore.GraphQL.Mutations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.Core
{
    public interface IBookService
    {
        IQueryable<Book> GetAll();
        Book Create(CreateBookInput inputBook);
        Book Delete(DeleteBookInput inputBook);
    }
}
