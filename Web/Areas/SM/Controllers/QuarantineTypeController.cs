using Common.Helpers;
using DataAccess;
using Entities;
using Entities.Enums;
using Entities.ViewModels;
using Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Controllers;
using Web.Filters;
namespace Web.Areas.SM.Controllers
{
    [Area("SM")]
    [Route("sm")]
    public class QuarantineTypeController : BaseController
    {
        public QuarantineTypeController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache)
        {
        }
        #region Index
        [Route("qt", Name = "QuarantineTypeIndex")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.QuarantineType, AppEnum.Covid })]

        public async Task<IActionResult> Index()
        {
            var models = await _repository.GetRepository<QuarantineType>().GetAllAsync();
            return View(models.OrderBy(o => o.OrdinalNumber));
        }
        #endregion
        #region Create
        [Route("qt/create", Name = "QuarantineTypeCreate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.QuarantineType, AppEnum.Covid })]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("qt/create")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.QuarantineType, AppEnum.Covid })]
        public async Task<IActionResult> Create(QuarantineTypeCreating model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string name = StringHelper.KillChars(model.Name);
                    var quarantineType = new QuarantineType()
                    {
                        Name = name,
                        OrdinalNumber = model.OrdinalNumber,
                        CreateBy = AccountId,
                        UpdateBy = AccountId,
                    };

                    int result = await _repository.GetRepository<QuarantineType>().CreateAsync(quarantineType, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Nhập Hình thức cách ly thành công!";
                        return RedirectToRoute("QuarantineTypeIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Nhập Hình thức cách ly không thành công!";
                        ModelState.AddModelError(string.Empty, "Nhập Hình thức cách ly không thành công!");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Đã xảy ra lỗi: " + ex.Message;
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
                    return View(model);
                }
            }
            else
            {
                ViewBag.Error = "Vui lòng nhập chính xác các thông tin!";
                ModelState.AddModelError(string.Empty, "Vui lòng nhập chính xác các thông tin!");
                return View(model);
            }
        }
        #endregion
        #region Update
        [Route("qt/update/{id}", Name = "QuarantineTypeUpdate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.QuarantineType, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id)
        {
            QuarantineType quarantineType = await _repository.GetRepository<QuarantineType>().ReadAsync(id);
            if (quarantineType == null)
            {
                TempData["Error"] = "Không tìm thấy Hình thức cách ly!";
                return RedirectToRoute("QuarantineTypeIndex");
            }
            var model = new QuarantineTypeUpdating()
            {
                Id = quarantineType.Id,
                Name = quarantineType.Name,
                OrdinalNumber = quarantineType.OrdinalNumber,
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [Route("qt/update/{id}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.QuarantineType, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id, QuarantineTypeUpdating model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    QuarantineType quarantineType = await _repository.GetRepository<QuarantineType>().ReadAsync(id);
                    if (quarantineType == null)
                    {
                        TempData["Error"] = "Không tìm thấy Hình thức cách ly!";
                        return RedirectToRoute("QuarantineTypeIndex");
                    }

                    string name = StringHelper.KillChars(model.Name);

                    quarantineType.Name = name;
                    quarantineType.OrdinalNumber = model.OrdinalNumber;
                    quarantineType.UpdateBy = AccountId;

                    int result = await _repository.GetRepository<QuarantineType>().UpdateAsync(quarantineType, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Cập nhật Hình thức cách ly thành công!";
                        return RedirectToRoute("QuarantineTypeIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Cập nhật Hình thức cách ly không thành công!";
                        ModelState.AddModelError(string.Empty, "Cập nhật Hình thức cách ly không thành công!");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Đã xảy ra lỗi: " + ex.Message;
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
                    return View(model);
                }
            }
            else
            {
                ViewBag.Error = "Vui lòng nhập chính xác các thông tin!";
                ModelState.AddModelError(string.Empty, "Vui lòng nhập chính xác các thông tin!");
                return View(model);
            }
        }
        #endregion
        #region Delete
        [HttpPost, ValidateAntiForgeryToken]
        [Route("qt/delete/{id?}", Name = "QuarantineTypeDelete")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.QuarantineType, AppEnum.Covid })]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                QuarantineType quarantineType = await _repository.GetRepository<QuarantineType>().ReadAsync(id);
                if (quarantineType != null)
                {
                    var any = await _repository.GetRepository<FCase>().AnyAsync(o => o.QuarantineTypeId == id);
                    if (any)
                    {
                        return Json(new { success = false, message = "Hình thức cách ly đang được sử dụng!" });
                    }

                    int result = await _repository.GetRepository<QuarantineType>().DeleteAsync(quarantineType, AccountId);
                    if (result > 0)
                    {
                        return Json(new { success = true, message = "Xóa Hình thức cách ly thành công!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Lỗi: Không xóa được Hình thức cách ly!" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Lỗi: Không tìm thấy Hình thức cách ly!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
        #endregion
    }
}
