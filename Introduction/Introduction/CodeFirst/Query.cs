using HotChocolate.Resolvers;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Introduction.CodeFirst
{
    public class Query
    {
        public string Hello() => "World!";
        public string HelloWithParam(string name) => $"Greetings {name}!";

        /* In this way you can get access to the resolver from HotChocolate:
        public string HellpWithParamAndResolver(IResolverContext context, string name)
        {
            return $"Greetings {name} {context.Service<FooService>().GetBar()}";
        }
        */
    }

    public class QueryType: ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(f => f.Hello()).Type<NonNullType<StringType>>();

            // we can add fields that are not based on our .NET type Query:
            descriptor.Field("foo").Type<StringType>().Resolver(() => "bar");

        }
    }
}
