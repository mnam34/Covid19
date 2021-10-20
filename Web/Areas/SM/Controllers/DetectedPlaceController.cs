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
    public class DetectedPlaceController : BaseController
    {
        public DetectedPlaceController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache)
        {
        }
        #region Index
        [Route("dp", Name = "DetectedPlaceIndex")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.DetectedPlace, AppEnum.Covid })]

        public async Task<IActionResult> Index()
        {
            var models = await _repository.GetRepository<DetectedPlace>().GetAllAsync();
            return View(models.OrderBy(o => o.OrdinalNumber));
        }
        #endregion
        #region Create
        [Route("dp/create", Name = "DetectedPlaceCreate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.DetectedPlace, AppEnum.Covid })]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("dp/create")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.DetectedPlace, AppEnum.Covid })]
        public async Task<IActionResult> Create(DetectedPlaceCreating model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string name = StringHelper.KillChars(model.Name);                  
                    var detectedPlace = new DetectedPlace()
                    {
                        Name = name,
                        OrdinalNumber = model.OrdinalNumber,
                        CreateBy = AccountId,
                        UpdateBy = AccountId,
                    };

                    int result = await _repository.GetRepository<DetectedPlace>().CreateAsync(detectedPlace, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Nhập Nơi phát hiện ca bệnh thành công!";
                        return RedirectToRoute("DetectedPlaceIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Nhập Nơi phát hiện ca bệnh không thành công!";
                        ModelState.AddModelError(string.Empty, "Nhập Nơi phát hiện ca bệnh không thành công!");
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
        [Route("dp/update/{id}", Name = "DetectedPlaceUpdate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.DetectedPlace, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id)
        {
            DetectedPlace detectedPlace = await _repository.GetRepository<DetectedPlace>().ReadAsync(id);
            if (detectedPlace == null)
            {
                TempData["Error"] = "Không tìm thấy Nơi phát hiện ca bệnh!";
                return RedirectToRoute("DetectedPlaceIndex");
            }
            var model = new DetectedPlaceUpdating()
            {
                Id = detectedPlace.Id,
                Name = detectedPlace.Name,
                OrdinalNumber = detectedPlace.OrdinalNumber,
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [Route("dp/update/{id}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.DetectedPlace, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id, DetectedPlaceUpdating model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DetectedPlace detectedPlace = await _repository.GetRepository<DetectedPlace>().ReadAsync(id);
                    if (detectedPlace == null)
                    {
                        TempData["Error"] = "Không tìm thấy Nơi phát hiện ca bệnh!";
                        return RedirectToRoute("DetectedPlaceIndex");
                    }

                    string name = StringHelper.KillChars(model.Name);
                   
                    detectedPlace.Name = name;
                    detectedPlace.OrdinalNumber = model.OrdinalNumber;
                    detectedPlace.UpdateBy = AccountId;

                    int result = await _repository.GetRepository<DetectedPlace>().UpdateAsync(detectedPlace, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Cập nhật Nơi phát hiện ca bệnh thành công!";
                        return RedirectToRoute("DetectedPlaceIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Cập nhật Nơi phát hiện ca bệnh không thành công!";
                        ModelState.AddModelError(string.Empty, "Cập nhật Nơi phát hiện ca bệnh không thành công!");
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
        [Route("dp/delete/{id?}", Name = "DetectedPlaceDelete")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.DetectedPlace, AppEnum.Covid })]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                DetectedPlace detectedPlace = await _repository.GetRepository<DetectedPlace>().ReadAsync(id);
                if (detectedPlace != null)
                {
                    var any = await _repository.GetRepository<FCase>().AnyAsync(o => o.DetectedPlaceId == id);
                    if (any)
                    {
                        return Json(new { success = false, message = "Nơi phát hiện ca bệnh đang được sử dụng!" });
                    }

                    int result = await _repository.GetRepository<DetectedPlace>().DeleteAsync(detectedPlace, AccountId);
                    if (result > 0)
                    {
                        return Json(new { success = true, message = "Xóa Nơi phát hiện ca bệnh thành công!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Lỗi: Không xóa được Nơi phát hiện ca bệnh!" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Lỗi: Không tìm thấy Nơi phát hiện ca bệnh!" });
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
