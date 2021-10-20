using Common.Helpers;
using DataAccess;
using Entities;
using Entities.Enums;
using Entities.ViewModels;
using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Web.Controllers;
using Web.Filters;
using Web.Helpers;
namespace Web.Areas.SM.Controllers
{
    [Area("SM")]
    [Route("sm")]
    public class AccountController : BaseController, IAppBase
    {
        #region General
        public AccountController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache) { }
        #endregion

        #region Index
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.Account, AppEnum.SystemManagement })]
        [Route("account", Name = "Account_Index")]
        public IActionResult Index()
        {
            //var divisions = new SharedController(_repository, _context, _cache).GetDivisions(0);
            //var divisions = new SharedController(_repository, _context, _cache).GetDivisions2(-1, 0);
            //var divisions = new SharedController(_repository, _context, _cache).GetDivisionGroup(true);
            //ViewData["Division"] = new SelectList(divisions, "Id", "Name", "", "GroupName");
            return View();
        }
        #endregion
        #region Nhập tài khoản mới
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.Account, AppEnum.SystemManagement })]
        [Route("account/create", Name = "Account_Create")]
        public IActionResult Create()
        {
            //var divisions = new SharedController(_repository, _context, _cache).GetDivisions(0);
            //var divisions = new SharedController(_repository, _context, _cache).GetDivisions2(-1, 0);
            //var divisions = new SharedController(_repository, _context, _cache).GetDivisionGroup();
            //ViewData["Division"] = new SelectList(divisions, "Id", "Name", "", "GroupName");
            //var positions = _repository.GetRepository<Position>().GetAll().OrderBy(o => o.OrdinalNumber);
            //ViewData["Position"] = new SelectList(positions, "Id", "Name", "");
            return View();
        }
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.Account, AppEnum.SystemManagement })]
        [HttpPost, ValidateAntiForgeryToken]
        [Route("account/create")]
        public async Task<IActionResult> Create(AccountCreate model)
        {
            //var divisions = new SharedController(_repository, _context, _cache).GetDivisions(0);
            //var divisions = new SharedController(_repository, _context, _cache).GetDivisions2(-1, 0);
            //var divisions = new SharedController(_repository, _context, _cache).GetDivisionGroup();
            //ViewData["Division"] = new SelectList(divisions, "Id", "Name", model.DivisionId, "GroupName");

            //var positions = _repository.GetRepository<Position>().GetAll().OrderBy(o => o.OrdinalNumber);
            //ViewData["Position"] = new SelectList(positions, "Id", "Name", model.PositionId);
            if (ModelState.IsValid)
            {
                var loginName = StringHelper.KillChars(model.LoginName);
                var name = StringHelper.KillChars(model.Name);
                var email = StringHelper.KillChars(model.Email);
                var password = StringHelper.KillChars(model.Password);

                var any1 = await _repository.GetRepository<Account>().AnyAsync(o => o.LoginName == loginName);
                if (any1)
                {
                    ModelState.AddModelError(string.Empty, "Tên truy cập đã được sử dụng, vui lòng chọn tên khác!");
                    ModelState.AddModelError("LoginName", "Tên truy cập đã được sử dụng, vui lòng chọn tên khác!");
                    return View(model);
                }
                var any2 = await _repository.GetRepository<Account>().AnyAsync(o => o.Email == email);
                if (any2)
                {
                    ModelState.AddModelError(string.Empty, "Địa chỉ email đã được sử dụng, vui lòng chọn email khác!");
                    ModelState.AddModelError("LoginName", "Địa chỉ email đã được sử dụng, vui lòng chọn email khác!");
                    return View(model);
                }
                var p = CommonHelper.StringToSHA512(CommonHelper.ConvertStringtoMD5(password).ToLower());
                var account = new Account()
                {
                    LoginName = loginName,
                    Name = name,
                    RealName = name,
                    Email = email,
                    AccessRight = true,
                    Password = p,
                    CreateBy = AccountId,
                    //DivisionId = model.DivisionId,
                    //PositionId = model.PositionId
                };
                //if (model.DivisionId.HasValue)
                //{
                //    var division = await _repository.GetRepository<Division>().ReadAsync(model.DivisionId.Value);
                //    if (division != null)
                //        account.CompanyId = division.CompanyId;
                //}
                int result = await _repository.GetRepository<Account>().CreateAsync(account, 0);

                if (result > 0)
                {
                    TempData["Success"] = "Tạo tài khoản mới thành công!";
                    /*
                    //Phân quyền
                    try
                    {
                        var roleId = 5;
                        if (model.PositionId.HasValue)
                        {
                            switch (model.PositionId.Value)
                            {
                                case 1:
                                    roleId = 2;
                                    break;
                                case 2:
                                    roleId = 3;
                                    break;
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                    roleId = 4;
                                    break;
                                default:
                                    roleId = 5;
                                    break;
                            }
                        }
                        var sr = new AccountRole()
                        {
                            AccountId = account.Id,
                            RoleId = roleId,
                            CreateBy = AccountId,
                            UpdateBy = AccountId,
                        };
                        var any = await _repository.GetRepository<Role>().AnyAsync(o => o.Id == roleId);
                        if (any)
                            await _repository.GetRepository<AccountRole>().CreateAsync(sr, 0);
                    }
                    catch { }
                    */
                    return RedirectToRoute("AccountRead", new { id = account.Id });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tạo tài khoản mới không thành công! Vui lòng kiểm tra và thử lại!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập chính xác các thông tin!");
                return View(model);
            }
        }
        #endregion
        #region  Danh sách tài khoản trả về JSON
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.Account, AppEnum.SystemManagement })]
        [Route("accounts-json", Name = "Account_Json")]
        public IActionResult Account_Json()
        {
            string drawReturn = "1";
            int skip = 0;
            int take = 10;

            string start = Request.Form["start"];//Đang hiển thị từ bản ghi thứ mấy
            string length = Request.Form["length"];//Số bản ghi mỗi trang
            string draw = Request.Form["draw"];//Số lần request bằng ajax (hình như chống cache)
            string key = Request.Form["search[value]"];//Ô tìm kiếm            
            string orderDir = Request.Form["order[0][dir]"];//Trạng thái sắp xếp xuôi hay ngược: asc/desc
            orderDir = string.IsNullOrEmpty(orderDir) ? "asc" : orderDir;
            string orderColumn = Request.Form["order[0][column]"];//Cột nào đang được sắp xếp (cột thứ mấy trong html table)
            orderColumn = string.IsNullOrEmpty(orderColumn) ? "1" : orderColumn;
            string orderKey = Request.Form["columns[" + orderColumn + "][data]"];//Lấy tên của cột đang được sắp xếp
            orderKey = string.IsNullOrEmpty(orderKey) ? "Id" : orderKey;
            orderKey = "AccessRight";
            orderDir = "desc";

            if (!string.IsNullOrEmpty(start))
                skip = Convert.ToInt16(start);
            if (!string.IsNullOrEmpty(length))
                take = Convert.ToInt16(length);
            if (!string.IsNullOrEmpty(draw))
                drawReturn = draw;
            string strDivision = "";
            long companyId = 0;
            long divisionId = 0;

            string filter1 = Request.Form["filter-1"];//Lọc đơn vị
            if (!string.IsNullOrEmpty(filter1))
                strDivision = filter1.ToString();
            
            if (!string.IsNullOrEmpty(strDivision))
            {
                var tmp = strDivision.Split('_');
                if (tmp.Length > 1)
                {
                   
                    long.TryParse(tmp[0], out companyId);
                }
                else
                {
                    long.TryParse(strDivision, out divisionId);
                }

            }

            //if (!string.IsNullOrEmpty(strDivision))


            //var orgIds = new List<long>();
            //if (com == "1")
            //{
            //    var divs = _repository.GetRepository<Division>().GetAll(o => o.CompanyId == companyId);
            //    if (divs != null && divs.Any())
            //    {
            //        orgIds.AddRange(divs.Select(o => o.Id));
            //        foreach (var item in divs)
            //        {
            //            var div = _repository.GetRepository<Division>().Read(item.Id);
            //            if (div != null && div.Divisions.Any())
            //            {
            //                orgIds.AddRange(div.Divisions.Select(o => o.Id));
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    var org = _repository.GetRepository<Division>().Read(divisionId);
            //    if (org != null && org.Divisions.Any())
            //    {
            //        orgIds = org.Divisions.Select(o => o.Id).ToList();
            //    }
            //}
            Paging paging = new()
            {
                TotalRecord = 0,
                Skip = skip,
                Take = take,
                OrderDirection = orderDir
            };

            var rows = _repository.GetRepository<Account>().GetAll(ref paging, orderKey, o => (
                   key == null ||
                   key == "" ||
                   o.Name.Contains(key) ||
                   o.RealName.Contains(key) ||
                   o.Email.Contains(key) ||
                   o.LoginName.Contains(key)
                   )
                  // && ((divisionId == 0 && companyId == 0) || o.DivisionId == divisionId || (orgIds.Any() && o.DivisionId.HasValue && orgIds.Contains(o.DivisionId.Value)))
                ).ToList();
            long stt = skip + 1;
            return Json(new
            {
                draw = drawReturn,
                recordsTotal = paging.TotalRecord,
                recordsFiltered = paging.TotalRecord,
                data = rows.Select(o => new
                {
                    o.Id,
                    STT = stt++,
                    o.Name,
                    o.RealName,
                    o.LoginName,
                    o.Email,
                    CreateDate = o.CreateDate.ToString("dd/MM/yyyy"),
                    CreateBy = new HtmlHelpersDB(_repository).GetCreateBy(o.CreateBy),
                    o.AccessRight,
                    TrangThai = o.AccessRight == true ? "Hoạt động" : "<i>Ngừng hoạt động</i>",
                    //DivisionId = new HtmlHelpersDB(_repository).GetDivisionName(o.DivisionId),
                    //PositionId = o.Position == null ? "" : o.Position.Name
                })
            });
        }
        #endregion
        #region Cập nhật thông tin người dùng
        /// <summary>
        /// Chi tiết thông tin tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.Account, AppEnum.SystemManagement })]
        [Route("account/update/{id?}", Name = "AccountRead")]
        public async Task<IActionResult> Read(long id)
        {
            ViewBag.Roles = await _repository.GetRepository<Role>().GetAllAsync();
            ViewBag.AccountRoles = await _repository.GetRepository<AccountRole>().GetAllAsync(o => o.AccountId == id);
            Account account = await _repository.GetRepository<Account>().ReadAsync(id);
            //var divisions = new SharedController(_repository, _context, _cache).GetDivisionGroup();
            //ViewData["Division"] = new SelectList(divisions, "Id", "Name", account.DivisionId, "GroupName");
            //var positions = _repository.GetRepository<Position>().GetAll().OrderBy(o => o.OrdinalNumber);
            //ViewData["Position"] = new SelectList(positions, "Id", "Name", account.PositionId);
            //var divisions3 = new SharedController(_repository, _context, _cache).GetDivisions(0);
            //ViewBag.Divisions3 = divisions3;
            //var companies = await _repository.GetRepository<Company>().GetAllAsync();
            //ViewBag.Companies = companies;
            //ViewBag.AccountDivisionWTs = await _repository.GetRepository<AccountDivisionWT>().GetAllAsync(o => o.AccountId == id);
            return View(account);
        }
        /// <summary>
        /// Cập nhật thông tin tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("account/update/{id?}")]
        [HttpPost, ValidateAntiForgeryToken]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.Account, AppEnum.SystemManagement })]
        public async Task<IActionResult> Read(long id, AccountUpdate model, IFormFile file)
        {
            //var divisions = new SharedController(_repository, _context, _cache).GetDivisionGroup();
            //ViewData["Division"] = new SelectList(divisions, "Id", "Name", model.DivisionId, "GroupName");
            //var positions = _repository.GetRepository<Position>().GetAll().OrderBy(o => o.OrdinalNumber);
            //ViewData["Position"] = new SelectList(positions, "Id", "Name", model.PositionId);
            if (ModelState.IsValid)
            {
                try
                {
                    if (id != model.Id)
                        return Json(new { success = false, message = "Không tìm thấy tài khoản người dùng!" });
                    string loginName = StringHelper.KillChars(model.LoginName);
                    string email = StringHelper.KillChars(model.Email);
                    //Kiểm tra trùng loginName
                    bool any1 = await _repository.GetRepository<Account>().AnyAsync(o => o.LoginName == loginName && o.Id != id);
                    if (any1)
                        return Json(new { success = false, message = "Tên truy cập đã được sử dụng. Vui lòng nhập tên truy cập khác!" });
                    //Kiểm tra trùng email
                    bool any2 = await _repository.GetRepository<Account>().AnyAsync(o => o.Email == email && o.Id != id);
                    if (any2)
                        return Json(new { success = false, message = "Địa chỉ email đã được sử dụng. Vui lòng nhập email khác!" });
                    Account account = await _repository.GetRepository<Account>().ReadAsync(id);
                    if (account == null)
                        return Json(new { success = false, message = "Không tìm thấy tài khoản người dùng!" });

                    //Cập nhật thông tin
                    string name = StringHelper.KillChars(model.Name);
                    account.LoginName = loginName;
                    account.Email = email;
                    account.Name = name;
                    account.RealName = name;
                    //account.DivisionId = model.DivisionId;
                    //account.PositionId = model.PositionId;
                    //if (model.DivisionId.HasValue)
                    //{
                    //    var division = await _repository.GetRepository<Division>().ReadAsync(model.DivisionId.Value);
                    //    if (division != null)
                    //        account.CompanyId = division.CompanyId;
                    //}
                    var maxFileSize = 10;
                    string photo = "";
                    try
                    {
                        string fullFilePath = "/Uploads/images/accounts/";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/images/accounts/");
                        try
                        {
                            if (!Directory.Exists(filePath))
                            {
                                DirectoryInfo di = Directory.CreateDirectory(filePath);
                            }
                        }
                        catch { }
                        if (file != null && file.Length > 0)
                        {
                            var contentLength = (file.Length / 1024) / 1024;
                            if (contentLength < maxFileSize)
                            {
                                string fileName = account.Id.ToString() + Path.GetExtension(file.FileName);
                                var path = Path.Combine(filePath, fileName);
                                if (System.IO.File.Exists(path))
                                {
                                    fileName = string.Format("{0}_{1}{2}", account.Id.ToString(), StringHelper.CreateRandomString(4), Path.GetExtension(file.FileName));
                                    path = Path.Combine(filePath, fileName);
                                }
                                using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }
                                photo = fullFilePath + fileName;
                            }

                        }
                    }
                    catch { }
                    if (!string.IsNullOrEmpty(photo))
                        account.ProfilePicture = photo;
                    int result = await _repository.GetRepository<Account>().UpdateAsync(account, AccountId);
                    if (id == AccountId)
                    {
                        HttpContext.Session.SetString("Email", account.Email);
                        HttpContext.Session.SetString("AccountName", account.Name);
                        HttpContext.Session.SetString("ProfilePicture", account.ProfilePicture ?? "");
                        HttpContext.Session.SetString("IsManageAccount", account.IsManageAccount.ToString());
                        HttpContext.Session.SetString("LoginName", account.LoginName);
                        HttpContext.Session.SetString("RealName", account.RealName);

                    }
                    return Json(new { success = true, message = "Cập nhật thông tin tài khoản thành công!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
                }
            }
            else
            {
                string msg = "<br />";
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        msg += error.ErrorMessage + "<br />";
                    }
                }
                return Json(new { success = false, message = "Vui lòng nhập chính xác các thông tin!\nLỗi: " + msg });
            }
        }
        #endregion        
        #region Phân quyền (phân vai trò) cho người dùng
        /// <summary>
        /// Phân quyền sử dụng chức năng
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.Account, AppEnum.SystemManagement })]
        [Route("account/mapping-role", Name = "AccountMappingRole")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountMappingRole(long accountId, string roles)
        {
            try
            {
                if (!string.IsNullOrEmpty(roles))
                {
                    string[] rolesChecked = Regex.Split(roles, ",");
                    var accountRoles = rolesChecked.Where(o => !string.IsNullOrWhiteSpace(o)).Select(o => new AccountRole()
                    {
                        RoleId = Convert.ToInt64(o),
                        AccountId = accountId,
                        CreateBy = AccountId,
                    }).ToList();
                    try
                    {
                        if (accountRoles.Any())
                        {
                            var accountRolesMapped = await _repository.GetRepository<AccountRole>().GetAllAsync(o => o.AccountId == accountId);
                            var toMap = accountRoles.Where(p => !accountRolesMapped.Any(p2 => p2.RoleId == p.RoleId));
                            var toUnMap = accountRolesMapped.Where(p => !accountRoles.Any(p2 => p2.RoleId == p.RoleId));
                            int result2 = await _repository.GetRepository<AccountRole>().DeleteAsync(toUnMap, AccountId);
                            int result = await _repository.GetRepository<AccountRole>().CreateAsync(toMap, AccountId);
                        }
                        else
                        {
                            int result = await _repository.GetRepository<AccountRole>().DeleteAsync(o => o.AccountId == accountId, AccountId);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = "Phân quyền không thành công. Lỗi: " + ex.Message });
                    }
                }
                else
                {
                    int result = await _repository.GetRepository<AccountRole>().DeleteAsync(o => o.AccountId == accountId, AccountId);
                }
                try
                {
                    var accountRoles2 = _repository.GetRepository<AccountRole>().GetAll(o => o.AccountId == accountId).Include(x => x.Role);
                    _cache.Set("AccountRoles_" + accountId, accountRoles2.ToList());
                }
                catch { }
                return Json(new { success = true, message = "Đã phân quyền thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Phân quyền không thành công. Lỗi: " + ex.Message });
            }
        }
        #endregion
        #region Xóa 1 tài khoản
        /// <summary>
        /// Xóa tài khoản người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.Account, AppEnum.SystemManagement })]
        [Route("account/delete/{id?}", Name = "AccountDelete")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                if (id == AccountId || id == 1)
                {
                    return Json(new { success = false, message = "Xóa tài khoản không thành công!" });
                }
                Account account = await _repository.GetRepository<Account>().ReadAsync(id);
                if (account != null)
                {
                    int result1 = await _repository.GetRepository<AccountRole>().DeleteAsync(o => o.AccountId == id, AccountId);
                    //Không xóa
                    //int result = await _repository.GetRepository<Account>().DeleteAsync(account, AccountId);
                    account.AccessRight = false;
                    int result = await _repository.GetRepository<Account>().UpdateAsync(account, AccountId);
                    if (result > 0)
                        return Json(new { success = true, message = "Xóa tài khoản thành công!" });
                    else
                        return Json(new { success = false, message = "Xóa tài khoản không thành công!" });
                }
                return Json(new { success = false, message = "Xóa tài khoản không thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Xóa tài khoản không thành công! Lỗi: " + ex.Message });
            }
        }
        #endregion
        #region Đổi mật khẩu cho người dùng
        /// <summary>
        /// Đổi mật khẩu cho người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.Account, AppEnum.SystemManagement })]
        [Route("account/change-password/{id?}", Name = "AccountChangePassword")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(long id, AccountChangePassword model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Account account = await _repository.GetRepository<Account>().ReadAsync(id);
                    if (account != null)
                    {
                        var password = StringHelper.KillChars(model.NewPassword);
                        var p = CommonHelper.StringToSHA512(CommonHelper.ConvertStringtoMD5(password).ToLower());
                        account.Password = p;
                        int result = await _repository.GetRepository<Account>().UpdateAsync(account, AccountId);
                        if (result > 0)
                            return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
                        else
                            return Json(new { success = false, message = "Đổi mật khẩu không thành công!" });
                    }
                    return Json(new { success = false, message = "Đổi mật khẩu không thành công!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Đổi mật khẩu không thành công! Lỗi: " + ex.Message });
                }
            }
            else
            {
                string msg = "<br />";
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        msg += error.ErrorMessage + "<br />";
                    }
                }
                return Json(new { success = false, message = "Vui lòng nhập chính xác các thông tin!\nLỗi: " + msg });
            }
        }
        #endregion
        #region Khôi phục tài khoản
        /// <summary>
        /// Khôi phục tài khoản người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.Account, AppEnum.SystemManagement })]
        [Route("account/recover/{id?}", Name = "AccountRecover")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Recover(long id)
        {
            try
            {
                Account account = await _repository.GetRepository<Account>().ReadAsync(id);
                if (account != null)
                {
                    account.AccessRight = true;
                    int result = await _repository.GetRepository<Account>().UpdateAsync(account, AccountId);
                    if (result > 0)
                        return Json(new { success = true, message = "Khôi phục tài khoản thành công!" });
                    else
                        return Json(new { success = false, message = "Khôi phục tài khoản không thành công!" });
                }
                return Json(new { success = false, message = "Khôi phục tài khoản không thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Khôi phục tài khoản không thành công! Lỗi: " + ex.Message });
            }
        }
        #endregion
        #region Xóa 1 tài khoản VĨNH VIỄN
        /// <summary>
        /// Xóa tài khoản người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.Account, AppEnum.SystemManagement })]
        [Route("account/delete-permanent/{id?}", Name = "AccountDeletePermanent")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePermanent(long id)
        {
            try
            {
                if (id == AccountId || id == 1)
                {
                    return Json(new { success = false, message = "Xóa tài khoản không thành công!" });
                }
                Account account = await _repository.GetRepository<Account>().ReadAsync(id);
                if (account != null)
                {
                    int result1 = await _repository.GetRepository<AccountRole>().DeleteAsync(o => o.AccountId == id, AccountId);
                    int result = await _repository.GetRepository<Account>().DeleteAsync(account, AccountId);
                    if (result > 0)
                        return Json(new { success = true, message = "Xóa tài khoản thành công!" });
                    else
                        return Json(new { success = false, message = "Xóa tài khoản không thành công!" });
                }
                return Json(new { success = false, message = "Xóa tài khoản không thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Xóa tài khoản không thành công! Lỗi: " + ex.Message });
            }
        }
        #endregion
        
    }
}
