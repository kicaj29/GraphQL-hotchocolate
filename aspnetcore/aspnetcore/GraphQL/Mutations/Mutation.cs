using aspnetcore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.Mutations
{
    public class Mutation
    {
        private readonly IBookService _bookService;

        public Mutation(IBookService bookService)
        {
            _bookService = bookService;
        }

        public Book CreateBook(CreateBookInput inputBook)
        {
            return _bookService.Create(inputBook);
        }

        public Book DeleteBook(DeleteBookInput inputBook)
        {
            return _bookService.Delete(inputBook);
        }
    }
}
