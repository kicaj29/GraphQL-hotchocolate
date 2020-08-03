using System;
using System.Collections.Generic;
using System.Text;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using HotChocolate.Utilities;
using HotChocolate.Configuration;
using HotChocolate.Language;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Introspection;
using HotChocolate.Types.Relay;
using HotChocolate.Stitching.Client;
using HotChocolate.Stitching.Delegation;
using HotChocolate.Stitching.Introspection;
using HotChocolate.Stitching.Merge;
using HotChocolate.Stitching.Utilities;

namespace Introduction.SchemaFirst
{
    public static class SchemaFirstRun
    {
        public static void RunWithResolver()
        {
            Console.WriteLine("---SchemaFirstRun:RunWithResolver---");

            var schema = SchemaBuilder.New()
                .AddDocumentFromString(
                    @"
                    type Query {
                        hello: String
                    }")
                .AddResolver("Query", "hello", () => "world(schema first)!!!")
                .Create();

            var executor = schema.MakeExecutable();

            Console.WriteLine(executor.Execute("{ hello }").ToJson() + Environment.NewLine);
        }

        public static void RunWithBindComplexType()
        {
            Console.WriteLine("---SchemaFirstRun:RunWithBindComplexType---");

            var schema = SchemaBuilder.New()
                .AddDocumentFromString(
                    @"
                    type Query {
                        hello: String
                    }")
                .BindComplexType<Query>()
                .Create();

            var executor = schema.MakeExecutable();

            Console.WriteLine(executor.Execute("{ hello }").ToJson() + Environment.NewLine);
        }

        public static void RunWithBindComplexTypeSpecifyField()
        {
            Console.WriteLine("---SchemaFirstRun:RunWithBindComplexTypeSpecifyField---");

            var schema = SchemaBuilder.New()
                .AddDocumentFromString(
                    @"
                    type Query {
                        myHello: String
                    }")
                .BindComplexType<Query>(c => c
                        .Field(f => f.Hello())
                        .Name("myHello"))
                    .Create();

            var executor = schema.MakeExecutable();

            Console.WriteLine(executor.Execute("{ myHello }").ToJson() + Environment.NewLine);
        }

        public static void RunWithBindComplexTypeQueryResolver()
        {
            Console.WriteLine("---SchemaFirstRun:RunWithBindComplexTypeQueryResolver---");

            var schema = SchemaBuilder.New()
                .AddDocumentFromString(
                    @"
                    type Query {
                        hello: String
                        greetings: String
                    }")
                .BindComplexType<Query>()
                .BindResolver<QueryResolvers>(c => c.To<Query>())
                .Create();

            var executor = schema.MakeExecutable();

            Console.WriteLine(executor.Execute("{ hello }").ToJson());
            Console.WriteLine(executor.Execute("{ greetings }").ToJson() + Environment.NewLine);
        }
    }
}
