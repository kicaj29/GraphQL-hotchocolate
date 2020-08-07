using aspnetcore.Core;
using aspnetcore.GraphQL.DataLoaders.ClassDataLoaders;
using HotChocolate;
using HotChocolate.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL
{
    public class AuthorResolver
    {
        private readonly IAuthorService _authorService;

        public AuthorResolver(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public Author GetAuthor(Book book, IResolverContext ctx)
        {
            return _authorService.GetAll().Where(a => a.Id == book.AuthorId).FirstOrDefault();
        }
    }
}
