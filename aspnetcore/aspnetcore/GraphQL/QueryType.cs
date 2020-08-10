using aspnetcore.Core;
using GreenDonut;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field("authorByCountry")
           .Argument("country", c => c.Type<NonNullType<StringType>>())
           .Type<NonNullType<ListType<NonNullType<AuthorType>>>>()
           .Resolver((IResolverContext ctx) =>
           {
               var svc = ctx.Service<IAuthorService>();

               IDataLoader<string, Author[]> authodDataLoader = ctx.GroupDataLoader<string, Author>(
                   "authorByCountry",
                    (IReadOnlyList<string> keys) =>
                    {
                        var t = Task.Run(() =>
                        {
                            return svc.GroupByCountry(keys);
                        });
                        t.ConfigureAwait(continueOnCapturedContext: false);
                        return t;
                    }
                   );

               return authodDataLoader.LoadAsync(ctx.Argument<string>("country"));
           });

            descriptor.Field("authorFromCache")
            .Argument("authorId", id => id.Type<NonNullType<IntType>>())
            .Type<AuthorType>()
            .Resolver((IResolverContext ctx) =>
            {
                var svc = ctx.Service<IAuthorService>();

                IDataLoader<int, Author> dataLoader = ctx.CacheDataLoader<int, Author>(
                    "authorId",
                    (int id) =>
                    {
                        var t = Task.Run(() =>
                        {
                            return svc.GetById(id);
                        });
                        t.ConfigureAwait(continueOnCapturedContext: false);
                        return t;
                    }
                    );

                return dataLoader.LoadAsync(ctx.Argument<int>("authorId"));
            });

            descriptor.Field("authorFetchOnce")
            .Argument("authorId", id => id.Type<NonNullType<IntType>>())
            .Type<AuthorType>()
            .Resolver((IResolverContext ctx) =>
            {
                var svc = ctx.Service<IAuthorService>();
                return ctx.FetchOnceAsync("authorId", 
                    () => {
                    var id = ctx.Argument<int>("authorId");
                    return Task.FromResult(svc.GetById(id));
                    }
                );
            });
        }
    }
}
