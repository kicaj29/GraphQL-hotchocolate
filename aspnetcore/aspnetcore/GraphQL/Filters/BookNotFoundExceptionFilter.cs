using aspnetcore.Core.Exceptions;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.Filters
{
    public class BookNotFoundExceptionFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            if (error.Exception is BookNotFoundException ex)
                return error.WithMessage($"Book with id {ex.BookId} not found");

            return error;
        }
    }
}
