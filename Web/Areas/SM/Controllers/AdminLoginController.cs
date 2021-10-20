using Common.Helpers;
using DataAccess;
using Entities;
using Entities.ViewModels;
using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Helpers;

namespace Web.Areas.SM.Controllers
{
    [Area("SM")]
    [Route("sm")]
    public class AdminLoginController : Controller
    {
        #region General

        private IRepository _repository { get; }
        private DataContext _context { get; }
        private IMemoryCache _cache;
        IConfiguration _iconfiguration;
        public AdminLoginController(IRepository repository, DataContext context, IMemoryCache memoryCache, IConfiguration iconfiguration)
        {
            _repository = repository;
            _context = context;
            _cache = memoryCache;
            _iconfiguration = iconfiguration;
        }
        #endregion
        #region Đăng nhập hệ thống
        [Route("~/login", Name = "AdminLogin_Index")]
        public IActionResult Index(string returnUrl = null)
        {
            SaveRequestLog();
            var userAgent = Request.Headers["User-Agent"];
            UserAgent ua = new(userAgent);
            var aa = ua.Browser;
            ViewBag.BrowserInfo = "Name=" + aa.Name + "; Version=" + aa.Version + "; Major=" + aa.Major;
            ViewBag.BrowserName = aa.Name;
            ViewData["ReturnUrl"] = returnUrl;
            HttpContext.Session.Clear();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        [Route("~/login")]
        public async Task<IActionResult> Index(LoginViewModel model, string returnUrl = null)
        {
            SaveRequestLog();

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                string loginName = StringHelper.KillChars(model.LoginName).Split('@')[0];
                string password = StringHelper.KillChars(model.Password);
                var p = CommonHelper.StringToSHA512(CommonHelper.ConvertStringtoMD5(password).ToLower());

                bool valid = false;
                // First way  
                string trainingMode = _iconfiguration.GetSection("Configs").GetSection("TrainingMode").Value;
                // Second way  
                string trainingMode1 = _iconfiguration.GetValue<string>("Configs:TrainingMode");
                if (trainingMode == "1")
                {
                    valid = _repository.GetRepository<Account>().Any(o => o.LoginName == loginName && o.AccessRight == true);
                }
                else
                {
                    valid = _repository.GetRepository<Account>().Any(o => o.LoginName == loginName && o.AccessRight == true && o.Password == p);
                }

                if (valid)
                {
                    var account = await _repository.GetRepository<Account>().ReadAsync(o => o.LoginName == loginName);
                    if (account != null)
                    {
                        if (account.AccessRight)
                        {
                            HttpContext.Session.SetString("Email", account.Email);
                            HttpContext.Session.SetString("AccountId", account.Id.ToString());
                            HttpContext.Session.SetString("AccountName", account.Name);
                            HttpContext.Session.SetString("ProfilePicture", account.ProfilePicture ?? "");
                            HttpContext.Session.SetString("IsManageAccount", account.IsManageAccount.ToString());
                            HttpContext.Session.SetString("LoginName", account.LoginName);
                            HttpContext.Session.SetString("RealName", account.RealName);

                            var accountRoles = _repository.GetRepository<AccountRole>().GetAll(o => o.AccountId == account.Id).Include(x => x.Account).Include(x => x.Role).ToList();
                            var moduleRoles = _repository.GetRepository<ModuleRole>().GetAll().ToList();
                            
                            _cache.Set("AccountRoles_" + account.Id, accountRoles.ToList());
                            _cache.Set("ModuleRoles", moduleRoles.ToList());
                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            ViewBag.Error = "Quí vị chưa được cấp quyền truy cập hệ thống!";
                            ModelState.AddModelError(string.Empty, "Quí vị chưa được cấp quyền truy cập hệ thống!");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Sai tên truy cập hoặc mật khẩu!");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Sai tên truy cập hoặc mật khẩu!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập đúng tên và mật khẩu!");
                return View(model);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl) && returnUrl != "/")
                return Redirect(returnUrl);
            else
                return RedirectToRoute("HomeIndex");
        }
        #endregion
        #region Đổi mật khẩu
        [Route("~/cw", Name = "UserChangePassword")]
        public IActionResult ChangePassword()
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountId))
            {
                //HttpContext.Response.Redirect("/login");
                return RedirectToRoute("AdminLogin_Index");
            }
            return View();
        }
        [Route("~/cw")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ChangePassword(UserChangePassword model)
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountId))
            {
                HttpContext.Response.Redirect("/login");
            }
            if (ModelState.IsValid)
            {

                string password = StringHelper.KillChars(model.OldPassword);
                string newPassword = StringHelper.KillChars(model.NewPassword);
                string confirmPassword = StringHelper.KillChars(model.ConfirmPassword);


                if (newPassword != confirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Mật khẩu mới và xác nhận mật khẩu mới không khớp. Vui lòng nhập lại!");
                    return View(model);
                }
                var id = long.Parse(accountId);
                var p = CommonHelper.StringToSHA512(CommonHelper.ConvertStringtoMD5(password).ToLower());
                var account = _repository.GetRepository<Account>().Read(o => o.Id == id && o.Password == p);
                if (account == null)
                {
                    ModelState.AddModelError(string.Empty, "Mật khẩu hiện tại không chính xác. Vui lòng nhập lại!");
                    return View(model);
                }
                var newPw = CommonHelper.StringToSHA512(CommonHelper.ConvertStringtoMD5(newPassword).ToLower());
                account.Password = newPw;
                int result = _repository.GetRepository<Account>().Update(account, id);
                return RedirectToRoute("UserChangePasswordSuccess");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập đúng các thông tin!");
                return View(model);
            }
        }
        [Route("~/cws", Name = "UserChangePasswordSuccess")]
        public IActionResult ChangePasswordSuccess()
        {
            HttpContext.Session.Clear();
            return View();
        }
        #endregion
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
        #region Khởi tạo hệ thống
        [Route("init/{key}")]
        public IActionResult Init(string key)
        {
            if (key != "nammv")
            {
                return RedirectToAction("Index", "AdminLogin");
            }

            if (_repository.GetRepository<Account>().Any(o => o.Id == 1))
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            var p = CommonHelper.StringToSHA512(CommonHelper.ConvertStringtoMD5("Thanhhoa@2021").ToLower());
            var account = new Account()
            {
                Name = "Admin",
                RealName = "Admin",
                Email = "mvnam.tho@gmail.com",
                LoginName = "admin",
                CreateBy = 0,
                UpdateBy = 0,
                AccessRight = true,
                Password = p,
            };
            _repository.GetRepository<Account>().CreateAsync(account, 0);
            //////////
            var role = new Role()
            {
                Code = "QT",
                Name = "Quản trị hệ thống",
                CreateBy = 0,
                UpdateBy = 0
            };
            _repository.GetRepository<Role>().CreateAsync(role, 0);
            //////////
            var roles = new Role[]
            {
                    new Role{Code="C1",Name="Cấp 1",UpdateBy=0, CreateBy=0},
                    new Role{Code="C2",Name="Cấp 2",UpdateBy=0, CreateBy=0},
                    new Role{Code="C3",Name="Cấp 3",UpdateBy=0, CreateBy=0},
                    new Role{Code="C4",Name="Cấp 4",UpdateBy=0, CreateBy=0},
            };
            _repository.GetRepository<Role>().CreateAsync(roles, 0);
            //////////
            var accountRole = new AccountRole()
            {
                AccountId = 1,
                RoleId = 1,
                CreateBy = 0,
                UpdateBy = 0
            };
            _repository.GetRepository<AccountRole>().CreateAsync(accountRole, 0);
            //////////
            var moduleRoles = new ModuleRole[]
           {
                    new ModuleRole{RoleId=1,ModuleCode="Account",Create=1,Read=1,Update=1,Delete=1,UpdateBy=0, CreateBy=0},
                    new ModuleRole{RoleId=1,ModuleCode="Role",Create=1,Read=1,Update=1,Delete=1,UpdateBy=0, CreateBy=0},
                    new ModuleRole{RoleId=1,ModuleCode="SystemInformation",Create=1,Read=1,Update=1,Delete=1,UpdateBy=0, CreateBy=0}
           };
            _repository.GetRepository<ModuleRole>().CreateAsync(moduleRoles, 0);
            return RedirectToAction("Index", "AdminLogin");
        }
        #endregion
    }
}
