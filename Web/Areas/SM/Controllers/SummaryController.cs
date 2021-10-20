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
    public class SummaryController : BaseController, IAppBase
    {
        #region General
        public SummaryController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache) { }
        #endregion
        #region Danh sách người dùng theo nhóm quyền
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.AssignedRole, AppEnum.SystemManagement })]
        [Route("summary/assigned-role", Name = "SummaryAssignedRole")]
        public async Task<IActionResult> AssignedRole()
        {
            var roles = await _repository.GetRepository<Role>().GetAllAsync();
            ViewData["Roles"] = new SelectList(roles.OrderBy(o => o.OrdinalNumber), "Id", "Name", "");
            return View();
        }
        #endregion
        #region  Danh sách tài khoản theo nhóm quyền trả về JSON
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.AssignedRole, AppEnum.SystemManagement })]
        [Route("summary/assigned-role-json")]
        public IActionResult AssignedRole_Json()
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
            //orderKey = "AccessRight";
            orderDir = "asc";

            if (!string.IsNullOrEmpty(start))
                skip = Convert.ToInt16(start);
            if (!string.IsNullOrEmpty(length))
                take = Convert.ToInt16(length);
            if (!string.IsNullOrEmpty(draw))
                drawReturn = draw;
            string strRoleId = "";
            long roleId = 0;
            string filter1 = Request.Form["filter-1"];//Lọc nhóm quyền
            if (!string.IsNullOrEmpty(filter1))
                strRoleId = filter1.ToString();
            if (!string.IsNullOrEmpty(strRoleId))
                _ = long.TryParse(strRoleId, out roleId);

            Paging paging = new()
            {
                TotalRecord = 0,
                Skip = skip,
                Take = take,
                OrderDirection = orderDir
            };

            var rows = _repository.GetRepository<AccountRole>().GetAll(ref paging, o => o.Role.OrdinalNumber, o => (
                     key == null ||
                     key == "" ||
                     o.Account.Name.Contains(key) ||
                     o.Account.RealName.Contains(key) ||
                     o.Account.Email.Contains(key) ||
                     o.Account.LoginName.Contains(key) ||
                     o.Role.Name.Contains(key) ||
                     o.Role.Code.Contains(key)
                     )
                     && (roleId == 0 || o.RoleId == roleId)
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
                    o.AccountId,
                    o.RoleId,
                    AccountName = o.Account.Name,
                    RoleCode = o.Role.Code,
                    RoleName = o.Role.Name,
                   // DivisionName = new HtmlHelpersDB(_repository).GetDivisionName(o.Account.DivisionId),
                    o.Account.LoginName,
                    CreateDate = o.CreateDate.ToString("dd/MM/yyyy"),
                    CreateBy = new HtmlHelpersDB(_repository).GetCreateBy(o.CreateBy),
                    o.Account.Email,
                    AccessRight = o.Account.AccessRight == true ? "Hoạt động" : "<i>Ngừng hoạt động</i>",
                    //PositionName = o.Account.Position == null ? "" : o.Account.Position.Name,
                })
            });
        }
        #endregion
        #region Xóa phân quyền nhóm
        /// <summary>
        /// Xóa/hủy phân quyền người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.AssignedRole, AppEnum.SystemManagement })]
        [Route("summary/assigned-role-delete-permanent/{id?}")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAssignedRole(long id)
        {
            try
            {
                var accountRole = await _repository.GetRepository<AccountRole>().ReadAsync(id);
                if (accountRole == null)
                    return Json(new { success = false, message = "Xóa phân quyền không thành công!" });
                if (accountRole.Role.Code == "QT" && accountRole.AccountId == 1)
                    return Json(new { success = false, message = "Xóa phân quyền không thành công!" });
                if (accountRole != null)
                {
                    int result = await _repository.GetRepository<AccountRole>().DeleteAsync(accountRole, AccountId);
                    if (result > 0)
                        return Json(new { success = true, message = "Xóa phân quyền thành công!" });
                    else
                        return Json(new { success = false, message = "Xóa phân quyền không thành công!" });
                }
                return Json(new { success = false, message = "Xóa phân quyền không thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Xóa phân quyền không thành công! Lỗi: " + ex.Message });
            }
        }
        #endregion
        
        
        


    }
}
