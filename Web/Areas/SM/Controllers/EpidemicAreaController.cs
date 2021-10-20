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
    public class EpidemicAreaController : BaseController
    {
        public EpidemicAreaController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache)
        {
        }
        #region Index
        [Route("ea", Name = "EpidemicAreaIndex")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.EpidemicArea, AppEnum.Covid })]

        public async Task<IActionResult> Index()
        {
            var models = await _repository.GetRepository<EpidemicArea>().GetAllAsync();
            return View(models.OrderByDescending(o => o.OutbreakDate));
        }
        #endregion
        #region Create
        [Route("ea/create", Name = "EpidemicAreaCreate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.EpidemicArea, AppEnum.Covid })]
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
        [Route("ea/create")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.EpidemicArea, AppEnum.Covid })]
        public async Task<IActionResult> Create(EpidemicAreaCreating model)
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
                    var strOutbreakDate = StringHelper.KillChars(model.OutbreakDate);
                    DateTime outbreakDate = DateTime.Now;
                    if (string.IsNullOrEmpty(strOutbreakDate))
                    {
                        ModelState.AddModelError("OutbreakDate", "Vui lòng nhập ngày bùng phát dịch tại Khu vực/vùng/điểm dịch này!");
                        return View(model);
                    }
                    if (!string.IsNullOrEmpty(strOutbreakDate))
                    {
                        try
                        {
                            outbreakDate = DateTime.ParseExact(strOutbreakDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    DateTime? lockdownDate = null;
                    DateTime? unLockdownDate = null;
                    DateTime? endDate = null;
                    var strLockdownDate = StringHelper.KillChars(model.LockdownDate);
                    var strUnLockdownDate = StringHelper.KillChars(model.UnLockdownDate);
                    var strEndDate = StringHelper.KillChars(model.EndDate);

                    if (!string.IsNullOrEmpty(strLockdownDate))
                    {
                        try
                        {
                            lockdownDate = DateTime.ParseExact(strLockdownDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    if (!string.IsNullOrEmpty(strUnLockdownDate))
                    {
                        try
                        {
                            unLockdownDate = DateTime.ParseExact(strUnLockdownDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    if (!string.IsNullOrEmpty(strEndDate))
                    {
                        try
                        {
                            endDate = DateTime.ParseExact(strEndDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    var epidemicArea = new EpidemicArea()
                    {
                        Name = name,
                        CommuneId = model.CommuneId,
                        DistrictId=model.DistrictId,
                        ProvinceId=model.ProvinceId,
                        OutbreakDate = outbreakDate,
                        LockdownDate = lockdownDate,
                        UnLockdownDate = unLockdownDate,
                        EndDate = endDate,
                        CreateBy = AccountId,
                        UpdateBy = AccountId,
                    };

                    int result = await _repository.GetRepository<EpidemicArea>().CreateAsync(epidemicArea, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Nhập Khu vực/vùng/điểm dịch thành công!";
                        return RedirectToRoute("EpidemicAreaIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Nhập Khu vực/vùng/điểm dịch không thành công!";
                        ModelState.AddModelError(string.Empty, "Nhập Khu vực/vùng/điểm dịch không thành công!");
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
        [Route("ea/update/{id}", Name = "EpidemicAreaUpdate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.EpidemicArea, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id)
        {
            EpidemicArea epidemicArea = await _repository.GetRepository<EpidemicArea>().ReadAsync(id);
            if (epidemicArea == null)
            {
                TempData["Error"] = "Không tìm thấy Khu vực/vùng/điểm dịch!";
                return RedirectToRoute("EpidemicAreaIndex");
            }
            var commune = await _repository.GetRepository<Commune>().ReadAsync(epidemicArea.CommuneId);
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            ViewData["Province"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", commune.District.ProvinceId);
            var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == commune.District.ProvinceId);
            ViewData["District"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", commune.DistrictId);
            var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == commune.DistrictId);
            ViewData["Commune"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name", epidemicArea.CommuneId);

            var model = new EpidemicAreaUpdating()
            {
                Id = epidemicArea.Id,
                Name = epidemicArea.Name,
                CommuneId = epidemicArea.CommuneId,
                DistrictId = commune.DistrictId,
                ProvinceId = commune.District.ProvinceId,
                OutbreakDate = epidemicArea.OutbreakDate.ToString("dd/MM/yyyy"),
                LockdownDate = epidemicArea.LockdownDate.HasValue ? epidemicArea.LockdownDate.Value.ToString("dd/MM/yyyy") : "",
                UnLockdownDate = epidemicArea.UnLockdownDate.HasValue ? epidemicArea.UnLockdownDate.Value.ToString("dd/MM/yyyy") : "",
                EndDate = epidemicArea.EndDate.HasValue ? epidemicArea.EndDate.Value.ToString("dd/MM/yyyy") : "",
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [Route("ea/update/{id}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.EpidemicArea, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id, EpidemicAreaUpdating model)
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
                    EpidemicArea epidemicArea = await _repository.GetRepository<EpidemicArea>().ReadAsync(id);
                    if (epidemicArea == null)
                    {
                        TempData["Error"] = "Không tìm thấy Khu vực/vùng/điểm dịch!";
                        return RedirectToRoute("EpidemicAreaIndex");
                    }

                    string name = StringHelper.KillChars(model.Name);
                    var strOutbreakDate = StringHelper.KillChars(model.OutbreakDate);
                    DateTime outbreakDate = DateTime.Now;
                    if (string.IsNullOrEmpty(strOutbreakDate))
                    {
                        ModelState.AddModelError("OutbreakDate", "Vui lòng nhập ngày bùng phát dịch tại Khu vực/vùng/điểm dịch này!");
                        return View(model);
                    }
                    if (!string.IsNullOrEmpty(strOutbreakDate))
                    {
                        try
                        {
                            outbreakDate = DateTime.ParseExact(strOutbreakDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    DateTime? lockdownDate = null;
                    DateTime? unLockdownDate = null;
                    DateTime? endDate = null;
                    var strLockdownDate = StringHelper.KillChars(model.LockdownDate);
                    var strUnLockdownDate = StringHelper.KillChars(model.UnLockdownDate);
                    var strEndDate = StringHelper.KillChars(model.EndDate);

                    if (!string.IsNullOrEmpty(strLockdownDate))
                    {
                        try
                        {
                            lockdownDate = DateTime.ParseExact(strLockdownDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    if (!string.IsNullOrEmpty(strUnLockdownDate))
                    {
                        try
                        {
                            unLockdownDate = DateTime.ParseExact(strUnLockdownDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    if (!string.IsNullOrEmpty(strEndDate))
                    {
                        try
                        {
                            endDate = DateTime.ParseExact(strEndDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    epidemicArea.Name = name;
                    epidemicArea.CommuneId = model.CommuneId;
                    epidemicArea.DistrictId = model.DistrictId;
                    epidemicArea.ProvinceId = model.ProvinceId;

                    epidemicArea.OutbreakDate = outbreakDate;
                    epidemicArea.LockdownDate = lockdownDate;
                    epidemicArea.UnLockdownDate = unLockdownDate;
                    epidemicArea.EndDate = endDate;

                    epidemicArea.UpdateBy = AccountId;

                    int result = await _repository.GetRepository<EpidemicArea>().UpdateAsync(epidemicArea, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Cập nhật Khu vực/vùng/điểm dịch thành công!";
                        return RedirectToRoute("EpidemicAreaIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Cập nhật Khu vực/vùng/điểm dịch không thành công!";
                        ModelState.AddModelError(string.Empty, "Cập nhật Khu vực/vùng/điểm dịch không thành công!");
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
        [Route("ea/delete/{id?}", Name = "EpidemicAreaDelete")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.EpidemicArea, AppEnum.Covid })]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                EpidemicArea epidemicArea = await _repository.GetRepository<EpidemicArea>().ReadAsync(id);
                if (epidemicArea != null)
                {
                    var any = await _repository.GetRepository<FCase>().AnyAsync(o => o.EpidemicAreaId == id);
                    if (any)
                    {
                        return Json(new { success = false, message = "Khu vực/vùng/điểm dịch đang được sử dụng!" });
                    }

                    int result = await _repository.GetRepository<EpidemicArea>().DeleteAsync(epidemicArea, AccountId);
                    if (result > 0)
                    {
                        return Json(new { success = true, message = "Xóa Khu vực/vùng/điểm dịch thành công!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Lỗi: Không xóa được Khu vực/vùng/điểm dịch!" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Lỗi: Không tìm thấy Khu vực/vùng/điểm dịch!" });
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
