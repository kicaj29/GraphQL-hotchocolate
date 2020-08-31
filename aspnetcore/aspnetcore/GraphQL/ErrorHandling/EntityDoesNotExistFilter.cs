using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.ErrorHandling
{
    public class EntityDoesNotExistFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            if (error.Exception is EntityDoesNotExistException ex)
            {
                return ErrorBuilder.FromError(error)
                    .SetMessage(ex.Message)
                    .SetException(null)
                    .SetCode(ErrorCodes.EntityDoesNotExist)
                    .SetExtension("ID", ex.Id)
                    .SetExtension("Type", ex.Type)
                    .RemoveExtension("stackTrace")
                    .Build();
            }
            return error;
        }
    }
}
