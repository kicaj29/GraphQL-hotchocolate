using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.ErrorHandling
{
    public enum ErrorTypes
    {
        THROW_NORMAL_EXCEPTION,
        DUPLICATE_KEY_EXCEPTION,
        ENTITY_DOES_NOT_EXISTS_EXCEPTION
    }
}
