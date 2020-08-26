using aspnetcore.Core;
using HotChocolate.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.Subscriptions
{
    public class Subscription
    {
        /// <summary>
        /// Subscription without parameter.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Review OnReview(IEventMessage message)
        {
            return (Review)message.Payload;
        }

        /// <summary>
        /// Subscription with parameter.
        /// Parameter can be used by the client (subscriber) to subscribe e.g. only for reviews for bookId = 5.
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Review OnReviewWithBookId(int bookId, IEventMessage message)
        {
            return (Review)message.Payload;
        }
    }
}
