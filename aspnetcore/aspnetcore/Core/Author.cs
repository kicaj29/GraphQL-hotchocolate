using HotChocolate.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.Core
{
    public class Author
    {
        public int Id { get; set; }
        [Authorize(Roles = new[] { "Managers" })]
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
    }
}
