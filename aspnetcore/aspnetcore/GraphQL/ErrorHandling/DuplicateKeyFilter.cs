using HotChocolate;
using HotChocolate.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.ErrorHandling
{
    public class DuplicateKeyFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            if (error.Exception is DuplicateKeyException ex)
            {
                return ErrorBuilder.FromError(error)
                    .SetMessage(ex.Message)
                    .SetException(null)
                    .SetCode(ErrorCodes.DuplicateKey)
                    .SetExtension("ID", ex.Id)
                    .SetExtension("Type", ex.Type)
                    .RemoveExtension("stackTrace")
                    .Build();
            }
            return error;
        }
    }
}
