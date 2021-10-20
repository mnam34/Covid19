using Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Web.Helpers
{
    public class RoleHelper
    {
        private IMemoryCache _cache { get; set; }
        public RoleHelper(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        public bool CheckPermission(ModuleEnum moduleEnum, ActionEnum actionEnum)
        {
            try
            {
                var context = AppHttpContext.Current;
                var strAccountId = context.Session.GetString("AccountId");
                if (strAccountId == null || strAccountId.ToString() == "") return false;
                var accountRoles = _cache.Get("AccountRoles_" + strAccountId) as List<AccountRole>;
                if (accountRoles == null || !accountRoles.Any()) return false;
                var moduleRoles1 = _cache.Get("ModuleRoles") as List<ModuleRole>;
                var moduleRoles = moduleRoles1.Where(o => accountRoles.Any(p => p.RoleId == o.RoleId));
                if (moduleRoles == null || !moduleRoles.Any()) return false;
                string moduleEnumString = moduleEnum.ToString();
                var tempModuleEnum = moduleRoles.Where(
                    o => o.ModuleCode.Equals(moduleEnumString, StringComparison.CurrentCultureIgnoreCase)).ToList();
                if (!tempModuleEnum.Any()) return false;

                switch (actionEnum)
                {
                    case ActionEnum.Read:
                        return tempModuleEnum.FirstOrDefault(a => a.Read == 1) != null;
                    case ActionEnum.Create:
                        return tempModuleEnum.FirstOrDefault(a => a.Create == 1) != null;
                    case ActionEnum.Update:
                        return tempModuleEnum.FirstOrDefault(a => a.Update == 1) != null;
                    case ActionEnum.Delete:
                        return tempModuleEnum.FirstOrDefault(a => a.Delete == 1) != null;
                    case ActionEnum.Verify:
                        return tempModuleEnum.FirstOrDefault(a => a.Verify == 1) != null;
                    case ActionEnum.Publish:
                        return tempModuleEnum.FirstOrDefault(a => a.Publish == 1) != null;
                    case ActionEnum.Confirm:
                        return tempModuleEnum.FirstOrDefault(a => a.Confirm == 1) != null;
                    case ActionEnum.Approve:
                        return tempModuleEnum.FirstOrDefault(a => a.Approve == 1) != null;
                    default:
                        return false;
                }
                //return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
