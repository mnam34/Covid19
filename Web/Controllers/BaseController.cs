using DataAccess;
using Entities;
using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Helpers;

namespace Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected int itemsPerPage = 10;
        protected IRepository _repository;
        protected DataContext _context;
        public IMemoryCache _cache;
        public BaseController(IRepository repository, DataContext context, IMemoryCache memoryCache)
        {
            _repository = repository;
            _context = context;
            _cache = memoryCache;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SaveRequestLog();
            var accountId = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountId))
            {
                var returnUrl = HttpContext.Request.Path;
                HttpContext.Response.Redirect("/login?returnUrl=" + returnUrl);
                filterContext.Result = new RedirectToRouteResult("AdminLogin_Index", new { returnUrl });
            }
            ViewData["AccountId"] = accountId;
            ViewData["AccountName"] = HttpContext.Session.GetString("AccountfName");
            ViewData["ProfilePicture"] = HttpContext.Session.GetString("ProfilePicture");
            ViewBag.LaQuanTri = LaQuanTri;

            ViewBag.SystemConfigs = SystemConfigs;
            ViewBag.IsSuperMaster = IsSuperMaster;
            ViewData["IsMobile"] = IsMobile;
        }
        private bool IsMobile
        {
            get
            {
                var isMobile = false;
                try
                {
                    var userAgent = Request.Headers["User-Agent"].ToString().ToLower();
                    isMobile = userAgent.Contains("mobi");
                }
                catch { }
                return isMobile;
            }
        }
        public bool IsSuperMaster
        {
            get
            {
                object obj = HttpContext.Session.GetString("IsManageAccount");
                bool _IsManage;
                if (obj == null)
                    _IsManage = false;
                else
                    _IsManage = Convert.ToBoolean(obj);
                return _IsManage;
            }
        }
        public long AccountId
        {
            get
            {
                object obj = HttpContext.Session.GetString("AccountId");
                long _objId;
                if (obj == null)
                    _objId = -1;
                else
                    _objId = Convert.ToInt64(obj.ToString());
                return _objId;
            }
        }
        public string AccountLoginName
        {
            get
            {
                object objLoginName = HttpContext.Session.GetString("LoginName");
                string _loginName;
                if (objLoginName == null)
                    _loginName = "";
                else
                    _loginName = objLoginName.ToString();
                return _loginName;
            }
        }
        public bool LaQuanTri
        {
            get
            {
                if (_cache.Get("AccountRoles_" + AccountId.ToString()) is not List<AccountRole> obj || !obj.Any()) return false;
                return obj.Any(o => o.Role.Code == "QT");
            }
        }

        public List<SystemConfig> SystemConfigs
        {
            get
            {
                _ = new List<SystemConfig>();
                List<SystemConfig> _divisionIds;
                if (_cache.Get("SystemConfig") is not List<SystemConfig> obj || !obj.Any())
                {
                    var tmp = _repository.GetRepository<SystemConfig>().GetAll();
                    if (tmp == null || !tmp.Any())
                        _divisionIds = new List<SystemConfig>();
                    else
                        _divisionIds = tmp.ToList();
                    _cache.Set("SystemConfig", tmp);
                }
                else
                    _divisionIds = obj;
                return _divisionIds;
            }
            set { _cache.Set("SystemConfig", value); }
        }
        #region SaveRequestLog
        private void SaveRequestLog()
        {
            var userAgent = Request.Headers["User-Agent"];
            UserAgent ua = new(userAgent);
            var aa = ua.Browser;

            try
            {
                string IpAddress = "";
                string FullComputerName = "";
                string ComputerName = "";
                string BrowserInfo = "";
                string RawUrl = "";
                string AbsoluteUri = "";
                try
                {
                    var httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();
                    var localIpAddress = httpConnectionFeature?.LocalIpAddress;
                    IpAddress = localIpAddress.ToString();
                    var ip = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                }
                catch { }
                try
                {
                    ComputerName = "OS=" + ua.OS.Name + "; Version=" + ua.OS.Version;
                }
                catch { }

                try
                {
                    FullComputerName = "OS=" + ua.OS.Name + "; Version=" + ua.OS.Version;
                }
                catch { }
                try
                {
                    BrowserInfo = "Name=" + aa.Name + "; Version=" + aa.Version + "; Major=" + aa.Major;
                }
                catch { }
                try
                {
                    var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
                    RawUrl = location.AbsoluteUri;
                }
                catch { }
                try
                {
                    var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
                    AbsoluteUri = location.AbsoluteUri;
                    //AbsoluteUri = Request.GetDisplayUrl();
                }
                catch { }
                try
                {
                    var r = new RequestLog()
                    {
                        Id = Guid.NewGuid(),
                        IpAddress = IpAddress,
                        FullComputerName = FullComputerName,
                        ComputerName = ComputerName,
                        BrowserInfo = BrowserInfo,
                        RawUrl = RawUrl,
                        AbsoluteUri = AbsoluteUri,
                    };
                    _repository.GetRepository<RequestLog>().Create(r, 0);
                }
                catch // (Exception ex)
                {
                    //throw ex;
                }
            }
            catch { }
        }
        #endregion
    }
}
