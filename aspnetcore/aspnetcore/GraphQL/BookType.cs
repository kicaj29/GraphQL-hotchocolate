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
    public class BookType : ObjectType<Book>
    {
        protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
        {
            descriptor.Field(b => b.Id).Type<IdType>();
            descriptor.Field(b => b.Title).Type<StringType>();
            descriptor.Field(b => b.Price).Type<DecimalType>();
            descriptor.Field(b => b.TimeStamp).Type<StringType>();
            descriptor.Field<AuthorResolver>(t => t.GetAuthor(default, default));

            descriptor.Field("authorFromBatch").Type<NonNullType<AuthorType>>().Resolver(
                (IResolverContext ctx) =>
                {
                    var aSvc = ctx.Service<IAuthorService>();                    
                    IDataLoader<int, Author> dataLoader = ctx.BatchDataLoader<int, Author>(
                        "AuthorById",
                        async (IReadOnlyList<int> keys) =>
                        {
                            return await Task.Run(() =>
                            {
                                var temp = aSvc.GetByIds(keys.ToList());
                                var dict = new Dictionary<int, Author>();
                                foreach (var item in temp)
                                {
                                    dict.Add(item.Id, item);
                                }
                                return dict;
                            });
                        }
                        );

                    return dataLoader.LoadAsync(ctx.Parent<Book>().AuthorId);
                }
                );
        }
    }
}
