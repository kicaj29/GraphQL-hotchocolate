using ApprovalTests;
using ApprovalTests.Reporters;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace aspnetcore.tests
{
    [UseReporter(typeof(DiffReporter))]
    public class BookQueryTest
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
                                                    query {
                                                        books {
                                                            nodes {
                                                                id
                                                                title
                                                                price
                                                            }
                                                        }
                                                    }
                                                ")
                                                .SetServices(provider)
                                                .Create();

            // act
            IExecutionResult result = executor.Execute(request);
            var resultJson = result.ToJson();

            // assert
            Approvals.Verify(resultJson);
        }
    }
}
