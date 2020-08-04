using ApprovalTests;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace aspnetcore.tests
{
    public class BookMutationTest
    {
        [Test]
        public void Test()
        {
            // arrange
            IServiceCollection services = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();
            var executor = provider.GetService<IQueryExecutor>();

            IReadOnlyQueryRequest request = QueryRequestBuilder.New()
                                    .SetQuery(@"
                                                    mutation {
                                                        createBook(inputBook: {
                                                                    authorId: 4, 
                                                                    price: 50, 
                                                                    title: ""Test book""}
                                                                ) {
                                                            price
                                                            title
                                                            authorId
                                                        }
                                                    }
                                                ")
                                    .SetServices(provider)
                                    .Create();

            // if needed we can also use params in mutation:

            /*IReadOnlyQueryRequest request = QueryRequestBuilder.New()
                                                .SetQuery(@"
                                                    mutation($title: String, $price: Decimal!, $authorId: Int!) {
                                                        createBook(inputBook: {
                                                                    authorId: $authorId, 
                                                                    price: $price, 
                                                                    title: $title}
                                                                ) {
                                                            price
                                                            title
                                                            authorId
                                                        }
                                                    }
                                                ")
                                                .SetServices(provider)
                                                .AddVariableValue("title", "Test book")
                                                .AddVariableValue("authorId", 4)
                                                .AddVariableValue("price", 50.0)
                                                .Create();*/

            // act
            IExecutionResult result = executor.Execute(request);
            var resultJson = result.ToJson();

            // assert
            Approvals.Verify(resultJson);
        }
    }
}
