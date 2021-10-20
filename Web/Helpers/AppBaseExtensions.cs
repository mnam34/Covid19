using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Helpers
{
    public static class AppBaseExtensions
    {
        public static string GetCurrentUser(this IAppBase app)
        {
            return "SM";
        }
    }
}
