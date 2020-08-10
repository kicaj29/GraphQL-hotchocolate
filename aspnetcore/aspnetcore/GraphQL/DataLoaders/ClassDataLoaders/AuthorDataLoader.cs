using aspnetcore.Core;
using GreenDonut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.DataLoaders.ClassDataLoaders
{
    public class AuthorDataLoader : DataLoaderBase<int, Author>
    {
        private IAuthorService _authorService;

        public AuthorDataLoader(IAuthorService authorService)
        {
            this._authorService = authorService;
        }

        protected override Task<IReadOnlyList<Result<Author>>> FetchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
        {
            var t = Task.Run(() =>
            {
                var temp = this._authorService.GetByIds(keys.ToList());
                // return new List<Result<Author>>(temp.Select(item => (Result<Author>.Resolve(item))));
                IReadOnlyList<Result<Author>> res =  new List<Result<Author>>(temp.Select(item => (Result<Author>.Resolve(item))));
                return res;
            });

            t.ConfigureAwait(continueOnCapturedContext: false);

            return t;
        }
    }
}
