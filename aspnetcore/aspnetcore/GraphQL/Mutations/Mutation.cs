using aspnetcore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Subscriptions;
using aspnetcore.GraphQL.Subscriptions;
using aspnetcore.Authentication;

namespace aspnetcore.GraphQL.Mutations
{
    public class Mutation
    {
        private readonly IBookService _bookService;

        public Mutation(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<Book> CreateBook(CreateBookInput inputBook, [Service]IEventSender eventSender)
        {            
            var book = _bookService.Create(inputBook);
            await eventSender.SendAsync(new OnReviewMessage(new Review()
            {
                BookId = book.Id,
                Stars = 1,
                Commentary = "Default comment created"
            }
            ));

            await eventSender.SendAsync(new OnReviewMessageWithBookId(book.Id, new Review()
            {
                BookId = book.Id,
                Stars = 1,
                Commentary = "Default comment created"
            }
));

            return book;
        }

        public Book DeleteBook(DeleteBookInput inputBook)
        {
            return _bookService.Delete(inputBook);
        }

        /// <summary>
        /// In normal situation this funcation is part of separte identity server.
        /// We mutation because generated token is different every time - it mutates.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="identityService"></param>
        /// <returns></returns>
        public Task<string> Authenticate(string email, string password, [Service] IIdentityService identityService) =>
            identityService.Authenticate(email, password);
    }
}
