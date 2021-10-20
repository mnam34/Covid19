using Entities;
using Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Helpers
{
    public class HtmlHelpersDB
    {
        private IRepository _repository;
        public HtmlHelpersDB(IRepository repository)
        {
            _repository = repository;
        }
        public string GetCreateBy(long id)
        {
            string by = "";
            if (id == 0)
                by = "System";
            try
            {
                var account = _repository.GetRepository<Account>().Read(id);
                if (account != null)
                    by = string.Format(@"{0} ({1})", account.Name, account.LoginName);
            }
            catch { }
            return by;
        }
        public string GetAddressByCommune(long id, bool shortName = true)
        {
            string addr = "";
            if (id == 0)
                addr = "";
            try
            {
                var commune = _repository.GetRepository<Commune>().Read(id);
                if (commune != null)
                    if (shortName)
                    {
                        var provinceName = commune.District.Province.Name.Replace("tỉnh", "", StringComparison.CurrentCultureIgnoreCase).Replace("thành phố", "", StringComparison.CurrentCultureIgnoreCase);
                        var sn = string.Join(".", provinceName.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToArray());
                        addr = string.Format(@"{0}, {1}, {2}",
                            commune.Name.Replace("thị trấn", "TT", StringComparison.CurrentCultureIgnoreCase).Replace("xã", "", StringComparison.CurrentCultureIgnoreCase).Replace("phường", "", StringComparison.CurrentCultureIgnoreCase),
                            commune.District.Name.Replace("thị xã", "TX", StringComparison.CurrentCultureIgnoreCase).Replace("huyện", "", StringComparison.CurrentCultureIgnoreCase).Replace("quận", "", StringComparison.CurrentCultureIgnoreCase).Replace("thành phố", "", StringComparison.CurrentCultureIgnoreCase),
                            sn
                            );
                    }
                    else
                        addr = string.Format(@"{0}, {1}, {2}", commune.Name, commune.District.Name, commune.District.Province.Name);
            }
            catch { }
            return addr;
        }

        //public string GetDivisionName(long? id)
        //{
        //    string orgName = "";
        //    if (!id.HasValue)
        //        orgName = "";
        //    else
        //    {
        //        try
        //        {
        //            var org = _repository.GetRepository<Division>().Read(id.Value);
        //            if (org != null)
        //                orgName = org.Name;
        //            if (org.ParentDivision != null)
        //                orgName = org.Name + ", " + org.ParentDivision.Name;
        //        }
        //        catch { }
        //    }
        //    return orgName;
        //}

    }
}
