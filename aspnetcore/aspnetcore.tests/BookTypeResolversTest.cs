using aspnetcore.Adapters;
using aspnetcore.Core;
using aspnetcore.GraphQL;
using HotChocolate;
using HotChocolate.DataLoader;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace aspnetcore.tests
{
    public class BookTypeResolversTest
    {
        [Test]
        [Ignore("There is problem with correct mocking IResolverContext")]
        public async Task TestAuthorResolver()
        {
            // arrange
            /*IServiceProvider sp = new ServiceCollection()
                            .AddDataLoaderRegistry()
                            .AddSingleton<IAuthorService, InMemoryAuthorService>()
                            .BuildServiceProvider();*/

            IServiceProvider sp = new ServiceCollection()
                .BuildServiceProvider();

            var x = sp.GetService<IResolverContext>();

            ISchema schema = Schema.Create(c =>
            {
                c.RegisterQueryType<QueryType>();
            });

            ObjectType type = schema.GetType<ObjectType>("QueryType");
            ObjectField field = type.Fields["authorByCountry"];

            Mock<IResolverContext> resolverContexMock = new Mock<IResolverContext>();
            resolverContexMock.Setup(c => c.Service<IAuthorService>()).Returns(new InMemoryAuthorService());
            var dl = new DataLoaderRegistry(sp);
            dl.Register<string, Author>("authorByCountry", (IReadOnlyList<string> keys) =>
            {
                List<Author> a = new List<Author>();

                return Task.FromResult(a.ToLookup(a1 => a1.Country));
            });
            resolverContexMock.Setup(c => c.Service<IEnumerable<IDataLoaderRegistry>>()).Returns(new List<DataLoaderRegistry>( new[] { dl }));
            // resolverContexMock.Setup(c => c.Service<IAuthorService>()).Returns(sp.GetService<IAuthorService>);
            // resolverContexMock.Setup(c => c.ScopedContextData.)
            // act
            var result = await field.Resolver(resolverContexMock.Object);

            // assert
        }
    }
}
