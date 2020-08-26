using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.Subscriptions
{
    public class SubscriptionType : ObjectType<Subscription>
    {
        protected override void Configure(IObjectTypeDescriptor<Subscription> descriptor)
        {
            descriptor.Field(t => t.OnReviewWithBookId(default, default))
                .Type<NonNullType<ReviewType>>()
                .Argument("bookId", arg => arg.Type<NonNullType<IntType>>());

            descriptor.Field(t => t.OnReview(default))
                .Type<NonNullType<ReviewType>>();
        }
    }
}
