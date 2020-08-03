using System;
using System.Collections.Generic;
using System.Text;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;

namespace Introduction.CodeFirst
{
    public static class CodeFirstRun
    {
        public static void RunPureCodeFirst()
        {
            Console.WriteLine("---CodeFirstRun:RunPureCodeFirst---");

            var schema = SchemaBuilder.New()
                .AddQueryType<Query>()
                .Create();

            var executor = schema.MakeExecutable();

            Console.WriteLine(executor.Execute("{ hello }").ToJson());
            Console.WriteLine(executor.Execute("{ helloWithParam(name: \"Jacek\") }").ToJson() + Environment.NewLine);
        }

        public static void RunCodeFirstBySchemaBuilder()
        {
            Console.WriteLine("---CodeFirstRun:RunCodeFirstBySchemaBuilder---");

            var schema = SchemaBuilder.New()
                .AddQueryType<Query>(d => d
                    .Field(f => f.Hello())
                    .Type<NonNullType<StringType>>())
                .Create();

            var executor = schema.MakeExecutable();

            Console.WriteLine(executor.Execute("{ hello }").ToJson());
            Console.WriteLine(executor.Execute("{ helloWithParam(name: \"Jacek\") }").ToJson() + Environment.NewLine);
        }

        public static void RunCodeFirstByObjectType()
        {
            Console.WriteLine("---CodeFirstRun:RunCodeFirstByObjectType---");

            var schema = SchemaBuilder.New()
                .AddQueryType<QueryType>()
                .Create();

            var executor = schema.MakeExecutable();

            Console.WriteLine(executor.Execute("{ hello }").ToJson());
            Console.WriteLine(executor.Execute("{ foo }").ToJson());
            Console.WriteLine(executor.Execute("{ helloWithParam(name: \"Jacek\") }").ToJson() + Environment.NewLine);
        }
    }
}
