﻿using aspnetcore.Core;
using HotChocolate.Types.Relay;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcore.GraphQL.DataLoaders.ClassDataLoaders;
using System.Threading;

namespace aspnetcore.GraphQL
{
    public class Query
    {
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;

        public Query(IAuthorService authorService, IBookService bookService)
        {
            _authorService = authorService;
            this._bookService = bookService;
        }

        [UsePaging(SchemaType = typeof(AuthorType))]
        [UseFiltering]
        public IQueryable<Author> Authors => _authorService.GetAll();

        [UsePaging(SchemaType = typeof(BookType))]
        [UseFiltering]
        public IQueryable<Book> Books => _bookService.GetAll();

        public Author Author(int id)
        {
            return _authorService.GetById(id);
        }

        public Task<Author> GetAuthorByIdAsync(
                int id,
                AuthorDataLoader dataLoader,
                CancellationToken cancellationToken) => dataLoader.LoadAsync(id, cancellationToken);
    }
}
