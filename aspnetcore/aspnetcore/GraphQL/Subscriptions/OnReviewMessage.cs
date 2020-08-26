using aspnetcore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.Subscriptions;
using HotChocolate.Language;

namespace aspnetcore.GraphQL.Subscriptions
{
    public class OnReviewMessage: EventMessage
    {
        public OnReviewMessage(Review review)
            : base(CreateEventDescription(), review)
        {

        }

        private static EventDescription CreateEventDescription()
        {
            return new EventDescription("onReview");
        }
    }
}
