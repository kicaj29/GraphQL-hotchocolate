using aspnetcore.Core;
using HotChocolate.Language;
using HotChocolate.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.Subscriptions
{
    public class OnReviewMessageWithBookId : EventMessage
    {
        public OnReviewMessageWithBookId(int bookId, Review review)
            : base(CreateEventDescription(bookId), review)
        {

        }

        private static EventDescription CreateEventDescription(int bookId)
        {
            return new EventDescription("onReviewWithBookId",
                new ArgumentNode("bookId",
                    new IntValueNode(bookId))
                );
        }
    }
}
