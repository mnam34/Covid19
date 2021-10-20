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
    public class QuarantinePlaceController : BaseController
    {
        public QuarantinePlaceController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache)
        {
        }
        #region Index
        [Route("qp", Name = "QuarantinePlaceIndex")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.QuarantinePlace, AppEnum.Covid })]

        public async Task<IActionResult> Index()
        {
            var models = await _repository.GetRepository<QuarantinePlace>().GetAllAsync();
            return View(models.OrderByDescending(o => o.OpenDate));
        }
        #endregion
        #region Create
        [Route("qp/create", Name = "QuarantinePlaceCreate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.QuarantinePlace, AppEnum.Covid })]
        public async Task<IActionResult> Create()
        {
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            ViewData["Province"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", 26);
            var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == 26);
            ViewData["District"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", 24);
            var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == 24);
            ViewData["Commune"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("qp/create")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.QuarantinePlace, AppEnum.Covid })]
        public async Task<IActionResult> Create(QuarantinePlaceCreating model)
        {
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            ViewData["Province"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.ProvinceId);
            var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == model.ProvinceId);
            ViewData["District"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.DistrictId);
            var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == model.DistrictId);
            ViewData["Commune"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.CommuneId);

            if (ModelState.IsValid)
            {
                try
                {
                    string name = StringHelper.KillChars(model.Name);
                    string addr = StringHelper.KillChars(model.Address);
                    var strOpenDate = StringHelper.KillChars(model.OpenDate);
                    DateTime openDate = DateTime.Now;
                    if (string.IsNullOrEmpty(strOpenDate))
                    {
                        ModelState.AddModelError("OpenDate", "Vui lòng nhập ngày bắt đầu hoạt động!");
                        return View(model);
                    }
                    if (!string.IsNullOrEmpty(strOpenDate))
                    {
                        try
                        {
                            openDate = DateTime.ParseExact(strOpenDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    DateTime? closeDate = null;
                    var strCloseDate = StringHelper.KillChars(model.CloseDate);
                    if (!string.IsNullOrEmpty(strCloseDate))
                    {
                        try
                        {
                            closeDate = DateTime.ParseExact(strCloseDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }

                    var quarantinePlace = new QuarantinePlace()
                    {
                        Name = name,
                        Address = addr,
                        Capacity = model.Capacity,
                        CommuneId = model.CommuneId,
                        DistrictId = model.DistrictId,
                        ProvinceId = model.ProvinceId,
                        OpenDate = openDate,
                        CloseDate = closeDate,
                        CreateBy = AccountId,
                        UpdateBy = AccountId,
                    };

                    int result = await _repository.GetRepository<QuarantinePlace>().CreateAsync(quarantinePlace, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Nhập Khu cách ly tập trung thành công!";
                        return RedirectToRoute("QuarantinePlaceIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Nhập Khu cách ly tập trung không thành công!";
                        ModelState.AddModelError(string.Empty, "Nhập Khu cách ly tập trung không thành công!");
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
        [Route("qp/update/{id}", Name = "QuarantinePlaceUpdate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.QuarantinePlace, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id)
        {
            QuarantinePlace quarantinePlace = await _repository.GetRepository<QuarantinePlace>().ReadAsync(id);
            if (quarantinePlace == null)
            {
                TempData["Error"] = "Không tìm thấy Khu cách ly tập trung!";
                return RedirectToRoute("QuarantinePlaceIndex");
            }
            var commune = await _repository.GetRepository<Commune>().ReadAsync(quarantinePlace.CommuneId);
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            ViewData["Province"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", commune.District.ProvinceId);
            var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == commune.District.ProvinceId);
            ViewData["District"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", commune.DistrictId);
            var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == commune.DistrictId);
            ViewData["Commune"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name", quarantinePlace.CommuneId);

            var model = new QuarantinePlaceUpdating()
            {
                Id = quarantinePlace.Id,
                Name = quarantinePlace.Name,
                Address = quarantinePlace.Address,
                Capacity = quarantinePlace.Capacity,
                CommuneId = quarantinePlace.CommuneId,
                DistrictId = commune.DistrictId,
                ProvinceId = commune.District.ProvinceId,
                OpenDate = quarantinePlace.OpenDate.ToString("dd/MM/yyyy"),
                CloseDate = quarantinePlace.CloseDate.HasValue ? quarantinePlace.CloseDate.Value.ToString("dd/MM/yyyy") : "",
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [Route("qp/update/{id}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.QuarantinePlace, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id, QuarantinePlaceUpdating model)
        {
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            ViewData["Province"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.ProvinceId);
            var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == model.ProvinceId);
            ViewData["District"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.DistrictId);
            var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == model.DistrictId);
            ViewData["Commune"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.CommuneId);

            if (ModelState.IsValid)
            {
                try
                {
                    QuarantinePlace quarantinePlace = await _repository.GetRepository<QuarantinePlace>().ReadAsync(id);
                    if (quarantinePlace == null)
                    {
                        TempData["Error"] = "Không tìm thấy Khu cách ly tập trung!";
                        return RedirectToRoute("QuarantinePlaceIndex");
                    }

                    string name = StringHelper.KillChars(model.Name);
                    string addr = StringHelper.KillChars(model.Address);
                    var strOpenDate = StringHelper.KillChars(model.OpenDate);
                    DateTime openDate = DateTime.Now;
                    if (string.IsNullOrEmpty(strOpenDate))
                    {
                        ModelState.AddModelError("OpenDate", "Vui lòng nhập ngày bắt đầu hoạt động!");
                        return View(model);
                    }
                    if (!string.IsNullOrEmpty(strOpenDate))
                    {
                        try
                        {
                            openDate = DateTime.ParseExact(strOpenDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    DateTime? closeDate = null;
                    var strCloseDate = StringHelper.KillChars(model.CloseDate);
                    if (!string.IsNullOrEmpty(strCloseDate))
                    {
                        try
                        {
                            closeDate = DateTime.ParseExact(strCloseDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    quarantinePlace.Name = name;
                    quarantinePlace.Address = addr;
                    quarantinePlace.Capacity = model.Capacity;
                    quarantinePlace.CommuneId = model.CommuneId;
                    quarantinePlace.DistrictId = model.DistrictId;
                    quarantinePlace.ProvinceId = model.ProvinceId;

                    quarantinePlace.OpenDate = openDate;
                    quarantinePlace.CloseDate = closeDate;
                    quarantinePlace.UpdateBy = AccountId;

                    int result = await _repository.GetRepository<QuarantinePlace>().UpdateAsync(quarantinePlace, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Cập nhật Khu cách ly tập trung thành công!";
                        return RedirectToRoute("QuarantinePlaceIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Cập nhật Khu cách ly tập trung không thành công!";
                        ModelState.AddModelError(string.Empty, "Cập nhật Khu cách ly tập trung không thành công!");
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
        [Route("qp/delete/{id?}", Name = "QuarantinePlaceDelete")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.QuarantinePlace, AppEnum.Covid })]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                QuarantinePlace quarantinePlace = await _repository.GetRepository<QuarantinePlace>().ReadAsync(id);
                if (quarantinePlace != null)
                {
                    var any = await _repository.GetRepository<FCase>().AnyAsync(o => o.QuarantinePlaceId == id);
                    if (any)
                    {
                        return Json(new { success = false, message = "Khu cách ly tập trung đang được sử dụng!" });
                    }

                    int result = await _repository.GetRepository<QuarantinePlace>().DeleteAsync(quarantinePlace, AccountId);
                    if (result > 0)
                    {
                        return Json(new { success = true, message = "Xóa Khu cách ly tập trung thành công!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Lỗi: Không xóa được Khu cách ly tập trung!" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Lỗi: Không tìm thấy Khu cách ly tập trung!" });
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
