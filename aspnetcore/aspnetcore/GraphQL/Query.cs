using aspnetcore.Core;
using HotChocolate.Types.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL
{
    public class Query
    {
        private readonly IAuthorService _authorService;
        public Query(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [UsePaging(SchemaType = typeof(AuthorType))]
        public IQueryable<Author> Authors => _authorService.GetAll();
    }
}
