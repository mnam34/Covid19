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
    public class TreatmentFacilityController : BaseController
    {
        public TreatmentFacilityController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache)
        {
        }
        #region Index
        [Route("tf", Name = "TreatmentFacilityIndex")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.TreatmentFacility, AppEnum.Covid })]

        public async Task<IActionResult> Index()
        {
            var models = await _repository.GetRepository<TreatmentFacility>().GetAllAsync();
            return View(models.OrderBy(o => o.OrdinalNumber));
        }
        #endregion
        #region Create
        [Route("tf/create", Name = "TreatmentFacilityCreate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.TreatmentFacility, AppEnum.Covid })]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("tf/create")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.TreatmentFacility, AppEnum.Covid })]
        public async Task<IActionResult> Create(TreatmentFacilityCreating model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string name = StringHelper.KillChars(model.Name);
                    string addr = StringHelper.KillChars(model.Address);
                    

                    var treatmentFacility = new TreatmentFacility()
                    {
                        Name = name,
                        Address = addr,
                        OrdinalNumber = model.OrdinalNumber,
                        CreateBy = AccountId,
                        UpdateBy = AccountId,
                    };

                    int result = await _repository.GetRepository<TreatmentFacility>().CreateAsync(treatmentFacility, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Nhập Cơ sở điều trị bệnh thành công!";
                        return RedirectToRoute("TreatmentFacilityIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Nhập Cơ sở điều trị bệnh không thành công!";
                        ModelState.AddModelError(string.Empty, "Nhập Cơ sở điều trị bệnh không thành công!");
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
        [Route("tf/update/{id}", Name = "TreatmentFacilityUpdate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.TreatmentFacility, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id)
        {
            TreatmentFacility treatmentFacility = await _repository.GetRepository<TreatmentFacility>().ReadAsync(id);
            if (treatmentFacility == null)
            {
                TempData["Error"] = "Không tìm thấy Cơ sở điều trị bệnh!";
                return RedirectToRoute("TreatmentFacilityIndex");
            }
            var model = new TreatmentFacilityUpdating()
            {
                Id = treatmentFacility.Id,
                Name = treatmentFacility.Name,
                Address = treatmentFacility.Address,
                OrdinalNumber = treatmentFacility.OrdinalNumber,
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [Route("tf/update/{id}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.TreatmentFacility, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id, TreatmentFacilityUpdating model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TreatmentFacility treatmentFacility = await _repository.GetRepository<TreatmentFacility>().ReadAsync(id);
                    if (treatmentFacility == null)
                    {
                        TempData["Error"] = "Không tìm thấy Cơ sở điều trị bệnh!";
                        return RedirectToRoute("TreatmentFacilityIndex");
                    }

                    string name = StringHelper.KillChars(model.Name);
                    string addr = StringHelper.KillChars(model.Address);
                   
                    treatmentFacility.Name = name;
                    treatmentFacility.Address = addr;
                    treatmentFacility.OrdinalNumber = model.OrdinalNumber;
                    treatmentFacility.UpdateBy = AccountId;

                    int result = await _repository.GetRepository<TreatmentFacility>().UpdateAsync(treatmentFacility, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Cập nhật Cơ sở điều trị bệnh thành công!";
                        return RedirectToRoute("TreatmentFacilityIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Cập nhật Cơ sở điều trị bệnh không thành công!";
                        ModelState.AddModelError(string.Empty, "Cập nhật Cơ sở điều trị bệnh không thành công!");
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
        [Route("tf/delete/{id?}", Name = "TreatmentFacilityDelete")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.TreatmentFacility, AppEnum.Covid })]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                TreatmentFacility treatmentFacility = await _repository.GetRepository<TreatmentFacility>().ReadAsync(id);
                if (treatmentFacility != null)
                {
                    var any = await _repository.GetRepository<FCase>().AnyAsync(o => o.TreatmentFacilityId == id);
                    if (any)
                    {
                        return Json(new { success = false, message = "Cơ sở điều trị bệnh đang được sử dụng!" });
                    }

                    int result = await _repository.GetRepository<TreatmentFacility>().DeleteAsync(treatmentFacility, AccountId);
                    if (result > 0)
                    {
                        return Json(new { success = true, message = "Xóa Cơ sở điều trị bệnh thành công!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Lỗi: Không xóa được Cơ sở điều trị bệnh!" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Lỗi: Không tìm thấy Cơ sở điều trị bệnh!" });
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
