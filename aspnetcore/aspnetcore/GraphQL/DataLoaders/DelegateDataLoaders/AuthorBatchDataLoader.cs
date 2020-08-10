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

        protected override Task<IReadOnlyDictionary<int, Author>> LoadBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
        {
            var t = Task.Run(() =>
            {
                var temp = this._svc.GetByIds(keys.ToList());
                var dict = new Dictionary<int, Author>();
                foreach (var item in temp)
                {
                    dict.Add(item.Id, item);
                }
                return (IReadOnlyDictionary<int, Author>)dict;
            });           
            
            t.ConfigureAwait(false);

            return t;
        }
    }
}
