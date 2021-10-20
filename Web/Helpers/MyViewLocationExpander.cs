using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
namespace Web.Helpers
{
    public class MyViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            yield return "~/Views/{1}/{0}.cshtml";
            yield return "~/Views/Shared/{0}.cshtml";

            yield return "~/Areas/{2}/Views/{1}/{0}.cshtml";
            yield return "~/Areas/{2}/Views/Shared/{0}.cshtml";

            //yield return "~/Areas/{2}/Views/HoSoCanBo/{0}.cshtml";
            //yield return "/Areas/{2}/Views/Categories2/{1}/{0}.cshtml";
            //yield return "/Areas/{2}/Views/Categories/WorkingSchedule/{1}/{0}.cshtml";
            //yield return "/Areas/{2}/Views/HoSoCanBo/{1}/{0}.cshtml";
            //yield return "/Areas/{2}/Views/QuaLyCanBo/{1}/{0}.cshtml";
            //yield return "/Areas/{2}/Views/Luong/{1}/{0}.cshtml";
            //yield return "/Areas/{2}/Views/LuanChuyenDieuDong/{1}/{0}.cshtml";
            //yield return "/Areas/{2}/Views/ThongKe/{1}/{0}.cshtml";
            //yield return "/Areas/{2}/Views/BaoCao/{1}/{0}.cshtml";

        }
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}
