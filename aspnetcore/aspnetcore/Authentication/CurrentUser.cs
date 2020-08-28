using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.Authentication
{
    public class CurrentUser
    {
        public string UserId { get; set; }
        public List<Tuple<string, string>> Claims { get; }

        public CurrentUser(string userId, List<Tuple<string, string>> claims)
        {
            UserId = userId;
            Claims = claims;
        }

        public class CurrentUserGlobalState : GlobalStateAttribute
        {
            public CurrentUserGlobalState() : base("currentUser")
            {
            }
        }
    }
}
