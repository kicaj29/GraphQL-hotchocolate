using aspnetcore.Core;
using HotChocolate.DataLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.DataLoaders.DelegateDataLoaders
{
    public class AuthorGroupedDataLoader: GroupedDataLoader<string, Author>
    {
        private readonly IAuthorService _authSvc;
        public AuthorGroupedDataLoader(IAuthorService authSvc)
        {
            this._authSvc = authSvc;
        }

        protected override async Task<ILookup<string, Author>> LoadGroupedBatchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                return this._authSvc.GroupByCountry(keys);
            }).ConfigureAwait(false);
        }
    }
}
