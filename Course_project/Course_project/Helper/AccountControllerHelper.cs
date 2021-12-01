using System;
using System.Collections.Generic;

namespace Course_project.Helper
{
    internal static class AccountControllerHelper
    {
        internal static string GetUserName(IEnumerable<dynamic> claims)
        {
            foreach (var claim in claims)
            {
                var uri = new Uri(claim.Type);
                if (uri.Segments[uri.Segments.Length - 1] == "givenname")
                    return claim.Value;
            }
            return null;
        }
    }
}
