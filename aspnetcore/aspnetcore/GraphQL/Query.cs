using aspnetcore.Core;
using HotChocolate.Types.Relay;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcore.GraphQL.DataLoaders.ClassDataLoaders;
using System.Threading;
using HotChocolate.Resolvers;
using aspnetcore.GraphQL.DataLoaders.DelegateDataLoaders;
using HotChocolate.AspNetCore.Authorization;
using static aspnetcore.Authentication.CurrentUser;
using aspnetcore.Authentication;
using System.Diagnostics;
using HotChocolate;
using aspnetcore.GraphQL.ErrorHandling;

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

        [Authorize(Policy = "super-boss-policy")]
        public Task<Author> GetAuthorByIdAsync(
                int id,
                AuthorDataLoader dataLoader,
                CancellationToken cancellationToken) => dataLoader.LoadAsync(id, cancellationToken);

        public Task<Author> GetAuthorByIdBatchAsync(
            int id,
            AuthorBatchDataLoader dataLoader,
            CancellationToken cancellationToken) => dataLoader.LoadAsync(id, cancellationToken);

        [Authorize(Roles = new[] { "Managers" })]
        public Task<Author[]> GetAuthorByCountry(
            string country,
            AuthorGroupedDataLoader dataLoader,
            CancellationToken cancellationToken,
            [CurrentUserGlobalState] CurrentUser user)
        {
            // We could pass user instance to other funcations to execute logic based on the claims
            foreach(var claim in user.Claims)
            {
                Debug.WriteLine($"Type: {claim.Item1}, Value: {claim.Item2}");
            }

            return dataLoader.LoadAsync(country, cancellationToken);
        }
        

        public Task<Author> GetAuthorFromCache(
            int authorId,
            AuthorCacheDataLoader dataLoader,
            CancellationToken cancellationToken
            ) => dataLoader.LoadAsync(authorId, cancellationToken);

        public string TestError(ErrorTypes errorType)
        {          
            switch (errorType)
            {
                case ErrorTypes.DUPLICATE_KEY_EXCEPTION:
                    throw new DuplicateKeyException(1, typeof(Book), "Entity with key already exists.");
                case ErrorTypes.ENTITY_DOES_NOT_EXISTS_EXCEPTION:
                    throw new EntityDoesNotExistException(2, typeof(Book), "Entity does not exists.");
                default:
                    throw new Exception($"Cannot find user with email");
            }
        }

    }
}
