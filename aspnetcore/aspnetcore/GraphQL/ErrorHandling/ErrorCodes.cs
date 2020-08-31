using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.GraphQL.ErrorHandling
{
    public static class ErrorCodes
    {
        public static string DuplicateKey { get { return "DUPLICATE_KEY"; } }
        public static string EntityDoesNotExist { get { return "ENTITY_DOES_NOT_EXIST"; } }
    }
}
