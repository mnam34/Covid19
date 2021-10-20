using Common.Helpers;
using DataAccess;
using Entities;
using Entities.Enums;
using Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Controllers;
using Web.Filters;
using Web.Helpers;

namespace Web.Areas.SM.Controllers
{
    [Area("SM")]
    [Route("sm")]
    public class RoleController : BaseController, IAppBase
    {
        public RoleController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache) { }
        #region Danh sách nhóm quyền
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.Role, AppEnum.SystemManagement })]
        [Route("role", Name = "RoleIndex")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Role> model = await _repository.GetRepository<Role>().GetAllAsync();
            return View(model);
        }
        #endregion
        #region Nhập nhóm quyền
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.Role, AppEnum.SystemManagement })]
        [Route("role/create", Name = "RoleCreate")]
        public IActionResult Create()
        {
            return View();
        }
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.Role, AppEnum.SystemManagement })]
        [Route("role/create")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var code = StringHelper.KillChars(model.Code);
                    var any = _repository.GetRepository<Role>().Any(o => o.Code == code);
                    if (any)
                    {
                        ModelState.AddModelError(string.Empty, "Mã đã được sử dụng. Vui lòng chọn mã khác!");
                        return View(model);
                    }
                    var role = new Role()
                    {
                        Code = code,
                        Name = StringHelper.KillChars(model.Name),
                        Description = StringHelper.KillChars(model.Description),
                        OrdinalNumber=model.OrdinalNumber,
                        CreateBy = AccountId,
                        UpdateBy = AccountId,
                    };

                    int result = await _repository.GetRepository<Role>().CreateAsync(role, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Nhập nhóm quyền thành công!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Nhập nhóm quyền mới không thành công! Vui lòng kiểm tra và thử lại!");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Lỗi:" + ex.Message);
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
        #region Cập nhật nhóm quyền
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.Role, AppEnum.SystemManagement })]
        [Route("role/update/{id?}", Name = "RoleUpdate")]
        public async Task<IActionResult> Update(long id)
        {
            Role role = await _repository.GetRepository<Role>().ReadAsync(id);
            if (role == null)
            {
                TempData["Error"] = "Không tìm thấy nhóm quyền!";
                return RedirectToRoute("RoleIndex");
            }
            return View(role);
        }

        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.Role, AppEnum.SystemManagement })]
        [Route("role/update/{id?}")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(long id, Role model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id != model.Id)
                    {
                        TempData["Error"] = "Không tìm thấy nhóm quyền!";
                        return RedirectToRoute("RoleIndex");
                    }
                    Role role = await _repository.GetRepository<Role>().ReadAsync(id);
                    if (role == null)
                    {
                        TempData["Error"] = "Không tìm thấy nhóm quyền!";
                        return RedirectToRoute("RoleIndex");
                    }

                    var code = StringHelper.KillChars(model.Code);
                    var name = StringHelper.KillChars(model.Name);
                    var any = _repository.GetRepository<Role>().Any(o => o.Code == code && o.Id != id);
                    if (any)
                    {
                        ModelState.AddModelError(string.Empty, "Mã đã được sử dụng. Vui lòng chọn mã khác!");
                        return View(model);
                    }
                    var any2 = _repository.GetRepository<Role>().Any(o => o.Name == name && o.Id != id);
                    if (any2)
                    {
                        ModelState.AddModelError(string.Empty, "Tên đã được sử dụng. Vui lòng chọn tên khác!");
                        return View(model);
                    }

                    role.Code = code;
                    role.Name = name;
                    role.Description = StringHelper.KillChars(model.Description);
                    role.OrdinalNumber = model.OrdinalNumber;
                    role.UpdateBy = AccountId;
                    int result = await _repository.GetRepository<Role>().UpdateAsync(role, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Cập nhật nhóm quyền thành công!";
                        return RedirectToRoute("RoleIndex");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Cập nhật nhóm quyền không thành công! Vui lòng kiểm tra và thử lại!");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Lỗi:" + ex.Message);
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
        #region Xóa nhóm quyền
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.Role, AppEnum.SystemManagement })]
        [Route("role/delete/{id?}", Name = "RoleDelete")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                Role role = await _repository.GetRepository<Role>().ReadAsync(id);
                if (role != null)
                {
                    //Nếu role đang được phân quyền thì cho xóa hay báo lỗi???
                    if (role.AccountRoles != null && role.AccountRoles.Any())
                    {
                        //Cho xóa thì xóa phân quyền trước
                        await _repository.GetRepository<AccountRole>().DeleteAsync(o => o.RoleId == id, AccountId);
                        //Không cho xóa thì báo lỗi
                        //return Json(new { success = false, message = "Lỗi: Nhóm quyền đang được sử dụng!" });
                    }
                    //Nếu role đã đươc khai báo module thì cho xóa hay báo lỗi???
                    if (role.ModuleRoles != null && role.ModuleRoles.Any())
                    {
                        //Cho xóa thì xóa module trước
                        await _repository.GetRepository<ModuleRole>().DeleteAsync(o => o.RoleId == id, AccountId);
                        //Không cho xóa thì báo lỗi
                        //return Json(new { success = false, message = "Lỗi: Nhóm quyền đang được sử dụng!" });
                    }
                    int result = await _repository.GetRepository<Role>().DeleteAsync(role, AccountId);
                    if (result > 0)
                    {
                        var accountRoles = _repository.GetRepository<AccountRole>().GetAll(o => o.AccountId == AccountId).Include(x => x.Role);
                        var moduleRoles = await _repository.GetRepository<ModuleRole>().GetAllAsync();
                        _cache.Set("AccountRoles_" + AccountId, accountRoles.ToList());
                        _cache.Set("ModuleRoles", moduleRoles.ToList());
                        return Json(new { success = true, message = "Xóa nhóm quyền thành công!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Lỗi: Không xóa được nhóm quyền!" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Lỗi: Không tìm thấy nhóm quyền!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
        #endregion
        #region Cập nhật chức năng cho nhóm quyền
        [Route("role/module/{id?}", Name = "RoleModuleRole")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.Role, AppEnum.SystemManagement })]
        public async Task<IActionResult> ModuleRole(long id)
        {
            ViewBag.RoleId = id;
            var role = await _repository.GetRepository<Role>().ReadAsync(id);
            ViewBag.RoleName = role.Name;
            ViewBag.ModuleRoles = await _repository.GetRepository<ModuleRole>().GetAllAsync();
            return View();
        }
        [Route("role/module/{id?}")]
        [HttpPost, ValidateAntiForgeryToken]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.Role, AppEnum.SystemManagement })]
        public async Task<IActionResult> ModuleRole()
        {
            var moduleCodes = ModuleEnum.Account.ToSelectListEnumRaw();
            long roleId = Convert.ToInt64(Request.Form["roleId"]);
            var moduleRoles = moduleCodes.Select(item => new ModuleRole()
            {
                RoleId = roleId,
                Read = (byte?)("on".Equals(Request.Form[item.Value + "_Read"]) ? 1 : 0),
                Create = (byte?)("on".Equals(Request.Form[item.Value + "_Create"]) ? 1 : 0),
                Update = (byte?)("on".Equals(Request.Form[item.Value + "_Update"]) ? 1 : 0),
                Delete = (byte?)("on".Equals(Request.Form[item.Value + "_Delete"]) ? 1 : 0),
                Approve = (byte?)("on".Equals(Request.Form[item.Value + "_Approve"]) ? 1 : 0),
                //Publish = (byte?)("on".Equals(Request.Form[item.Value + "_Publish"]) ? 1 : 0),
                CreateBy = AccountId,
                ModuleCode = item.Value
            }).ToList();
            if (moduleRoles != null && moduleRoles.Any())
            {
                foreach (var item in moduleRoles)
                {
                    ModuleRole moduleRole = await _repository.GetRepository<ModuleRole>().ReadAsync(o => o.RoleId == roleId && o.ModuleCode == item.ModuleCode);
                    if (moduleRole != null)
                    {
                        moduleRole.Read = item.Read;
                        moduleRole.Create = item.Create;
                        moduleRole.Update = item.Update;
                        moduleRole.Delete = item.Delete;
                        moduleRole.Approve = item.Approve;
                        //moduleRole.Publish = item.Publish;
                        moduleRole.UpdateDate = DateTime.Now;
                        moduleRole.UpdateBy = AccountId;
                        await _repository.GetRepository<ModuleRole>().UpdateAsync(moduleRole, AccountId);
                    }
                    else
                    {
                        await _repository.GetRepository<ModuleRole>().CreateAsync(item, AccountId);
                    }
                }
            }
            var accountRoles = _repository.GetRepository<AccountRole>().GetAll(o => o.AccountId == AccountId).Include(x => x.Role);
            var moduleRoles1 = _repository.GetRepository<ModuleRole>().GetAll().ToList();
            _cache.Set("AccountRoles_" + AccountId, accountRoles.ToList());
            _cache.Set("ModuleRoles", moduleRoles1.ToList());
            TempData["Success"] = "Đã ghi nhận thành công!";
            return RedirectToRoute("RoleModuleRole", new { id = roleId });
        }
        #endregion
    }
}
