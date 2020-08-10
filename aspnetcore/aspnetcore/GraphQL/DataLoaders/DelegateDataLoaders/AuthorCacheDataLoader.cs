using aspnetcore.Core;
using HotChocolate.DataLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.DataLoaders.DelegateDataLoaders
{
    public class AuthorCacheDataLoader : CacheDataLoader<int, Author>
    {
        private IAuthorService _svc;


        public AuthorCacheDataLoader(IAuthorService svc):base(10)
        {
            this._svc = svc;
        }

        protected override Task<Author> LoadSingleAsync(int key, CancellationToken cancellationToken)
        {
            var t = Task.Run(() =>
            {
                return this._svc.GetById(key);
            });
            t.ConfigureAwait(continueOnCapturedContext: false);

            return t;
        }
    }
}
