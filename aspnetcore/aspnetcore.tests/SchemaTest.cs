using ApprovalTests;
using ApprovalTests.Reporters;
using aspnetcore.GraphQL;
using HotChocolate;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace aspnetcore.tests
{
    [UseReporter(typeof(DiffReporter))]
    public class SchemaTest
    {
        [Test]
        public void TestQuerySchema()
        {
            // arrange
            ISchema schema = Schema.Create(c =>
            {
                c.RegisterQueryType<Query>();
            });

            // act
            var schemaSDL = schema.ToString();

            // assert
            Approvals.Verify(schemaSDL);
        }

        [Test]
        public void TestQueryTypeSchema()
        {
            // arrange
            ISchema schema = Schema.Create(c =>
            {
                c.RegisterQueryType<QueryType>();
            });

            // act
            var schemaSDL = schema.ToString();

            // assert
            Approvals.Verify(schemaSDL);
        }
    }
}
