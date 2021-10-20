using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using Web.Helpers;
namespace Web.Filters
{
    public class ValidationPermission : ActionFilterAttribute
    {
        private ModuleEnum _Module { get; set; }
        private ActionEnum _Action { get; set; }
        private AppEnum _App { get; set; }
        private IMemoryCache _cache { get; set; }
        public ValidationPermission(ActionEnum Action, ModuleEnum Module, AppEnum App, IMemoryCache memoryCache)
        {
            _Action = Action;
            _Module = Module;
            _App = App;
            _cache = memoryCache;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = AppHttpContext.Current;
            var accountId = context.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountId))
            {
                context.Response.Redirect("/login");
                filterContext.Result = new RedirectToRouteResult("AdminLogin_Index", null);
            }
            else
            {
                if (_Action == ActionEnum.NoCheck && _Module == ModuleEnum.NoCheck)
                {

                }
                else
                {
                    var result = new RoleHelper(_cache).CheckPermission(_Module, _Action);
                    if (!result)
                        try
                        {
                            filterContext.Result = new RedirectToRouteResult("SharedNoPermission", null);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            filterContext.Result = new RedirectToRouteResult("SharedNoPermission", null);
                        }
                }
            }
        }
    }
}
