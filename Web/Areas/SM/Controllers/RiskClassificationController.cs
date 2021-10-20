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
    public class RiskClassificationController : BaseController
    {
        public RiskClassificationController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache)
        {
        }
        #region Index
        [Route("rc", Name = "RiskClassificationIndex")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.RiskClassification, AppEnum.Covid })]

        public async Task<IActionResult> Index()
        {
            var models = await _repository.GetRepository<RiskClassification>().GetAllAsync();
            return View(models.OrderBy(o => o.OrdinalNumber));
        }
        #endregion
        #region Create
        [Route("rc/create", Name = "RiskClassificationCreate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.RiskClassification, AppEnum.Covid })]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("rc/create")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.RiskClassification, AppEnum.Covid })]
        public async Task<IActionResult> Create(RiskClassificationCreating model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string name = StringHelper.KillChars(model.Name);
                    var riskClassification = new RiskClassification()
                    {
                        Name = name,
                        OrdinalNumber = model.OrdinalNumber,
                        CreateBy = AccountId,
                        UpdateBy = AccountId,
                    };

                    int result = await _repository.GetRepository<RiskClassification>().CreateAsync(riskClassification, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Nhập Mức độ nguy cơ nhiễm bệnh thành công!";
                        return RedirectToRoute("RiskClassificationIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Nhập Mức độ nguy cơ nhiễm bệnh không thành công!";
                        ModelState.AddModelError(string.Empty, "Nhập Mức độ nguy cơ nhiễm bệnh không thành công!");
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
        [Route("rc/update/{id}", Name = "RiskClassificationUpdate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.RiskClassification, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id)
        {
            RiskClassification riskClassification = await _repository.GetRepository<RiskClassification>().ReadAsync(id);
            if (riskClassification == null)
            {
                TempData["Error"] = "Không tìm thấy Mức độ nguy cơ nhiễm bệnh!";
                return RedirectToRoute("RiskClassificationIndex");
            }
            var model = new RiskClassificationUpdating()
            {
                Id = riskClassification.Id,
                Name = riskClassification.Name,
                OrdinalNumber = riskClassification.OrdinalNumber,
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [Route("rc/update/{id}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.RiskClassification, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id, RiskClassificationUpdating model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    RiskClassification riskClassification = await _repository.GetRepository<RiskClassification>().ReadAsync(id);
                    if (riskClassification == null)
                    {
                        TempData["Error"] = "Không tìm thấy Mức độ nguy cơ nhiễm bệnh!";
                        return RedirectToRoute("RiskClassificationIndex");
                    }

                    string name = StringHelper.KillChars(model.Name);

                    riskClassification.Name = name;
                    riskClassification.OrdinalNumber = model.OrdinalNumber;
                    riskClassification.UpdateBy = AccountId;

                    int result = await _repository.GetRepository<RiskClassification>().UpdateAsync(riskClassification, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Cập nhật Mức độ nguy cơ nhiễm bệnh thành công!";
                        return RedirectToRoute("RiskClassificationIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Cập nhật Mức độ nguy cơ nhiễm bệnh không thành công!";
                        ModelState.AddModelError(string.Empty, "Cập nhật Mức độ nguy cơ nhiễm bệnh không thành công!");
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
        [Route("rc/delete/{id?}", Name = "RiskClassificationDelete")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.RiskClassification, AppEnum.Covid })]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                RiskClassification riskClassification = await _repository.GetRepository<RiskClassification>().ReadAsync(id);
                if (riskClassification != null)
                {
                    var any = await _repository.GetRepository<FCase>().AnyAsync(o => o.RiskClassificationId == id);
                    if (any)
                    {
                        return Json(new { success = false, message = "Mức độ nguy cơ nhiễm bệnh đang được sử dụng!" });
                    }

                    int result = await _repository.GetRepository<RiskClassification>().DeleteAsync(riskClassification, AccountId);
                    if (result > 0)
                    {
                        return Json(new { success = true, message = "Xóa Mức độ nguy cơ nhiễm bệnh thành công!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Lỗi: Không xóa được Mức độ nguy cơ nhiễm bệnh!" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Lỗi: Không tìm thấy Mức độ nguy cơ nhiễm bệnh!" });
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
