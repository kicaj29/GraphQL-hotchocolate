using aspnetcore.Core;
using HotChocolate.DataLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.DataLoaders.DelegateDataLoaders
{
    public class AuthorBatchDataLoader : BatchDataLoader<int, Author>
    {
        private IAuthorService _svc;

        public AuthorBatchDataLoader(IAuthorService svc)
        {
            this._svc = svc;
        }

        protected override async Task<IReadOnlyDictionary<int, Author>> LoadBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var temp = this._svc.GetByIds(keys.ToList());
                var dict = new Dictionary<int, Author>();
                foreach(var item in temp)
                {
                    dict.Add(item.Id, item);
                }
                return dict;
            }).ConfigureAwait(false);
        }
    }
}
