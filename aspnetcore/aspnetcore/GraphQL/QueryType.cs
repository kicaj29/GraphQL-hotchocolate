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
                    async (IReadOnlyList<string> keys) =>
                    {
                        return await Task.Run(() =>
                        {
                            return svc.GroupByCountry(keys);
                        });
                    }
                   );

               return authodDataLoader.LoadAsync(ctx.Argument<string>("country"));
           });
        }
    }
}
