using aspnetcore.Core;
using HotChocolate;
using HotChocolate.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL
{
    public class BookResolver
    {
        private readonly IBookService _bookService;

        public BookResolver(IBookService bookService)
        {
            _bookService = bookService;
        }
        public IEnumerable<Book> GetAuthorBooks(Author author, IResolverContext ctx)
        {
            return _bookService.GetAll().Where(b => b.AuthorId == author.Id);
        }
    }
}
