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
    public class FCaseController : BaseController
    {
        #region General
        public FCaseController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache) { }
        #endregion
        #region Danh sách ca F0
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.FCase, AppEnum.Covid })]
        [Route("fcases", Name = "FCaseIndex")]
        public async Task<IActionResult> Index()
        {
            var epidemicAreas = await _repository.GetRepository<EpidemicArea>().GetAllAsync();
            var epidemicAreaList = epidemicAreas
                .OrderBy(o => o.Commune.District.ProvinceId)
                .ThenBy(o => o.Commune.DistrictId)
                .ThenBy(o => o.CommuneId)
                .ThenByDescending(o => o.OutbreakDate)
                .Select(o => new EpidemicAreaList()
                {
                    Id = o.Id,
                    Name = o.Name + ", " + GetAddressByCommune(o.CommuneId)
                });
            ViewData["EpidemicArea"] = new SelectList(epidemicAreaList, "Id", "Name");
            return View();
        }
        #endregion
        #region  Danh sách F0 JSON
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.FCase, AppEnum.Covid })]
        [Route("fcases-json", Name = "FCaseJson")]
        public IActionResult FCase_Json()
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
            orderKey = "F0Date";
            orderDir = "desc";

            if (!string.IsNullOrEmpty(start))
                skip = Convert.ToInt16(start);
            if (!string.IsNullOrEmpty(length))
                take = Convert.ToInt16(length);
            if (!string.IsNullOrEmpty(draw))
                drawReturn = draw;
            string strEA = "";
            long eaId = 0;
            string filter1 = Request.Form["filter-1"];//Lọc đơn vị
            if (!string.IsNullOrEmpty(filter1))
                strEA = filter1.ToString();
            if (!string.IsNullOrEmpty(strEA))
            {
                long.TryParse(strEA, out eaId);
            }

            Paging paging = new()
            {
                TotalRecord = 0,
                Skip = skip,
                Take = take,
                OrderDirection = orderDir
            };

            var rows = _repository.GetRepository<FCase>().GetAll(ref paging, orderKey, o => (
                   key == null ||
                   key == "" ||
                   o.Name.Contains(key) ||
                   o.Address.Contains(key) ||
                   o.Code.Contains(key)
                   )
                   && o.IsF0
                   && (eaId == 0 || (o.EpidemicAreaId == eaId))
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
                    o.Code,
                    Address = o.Address + ", " + GetAddressByCommune(o.AddressCommuneId),
                    F0Date = o.F0Date.HasValue ? o.F0Date.Value.ToString("dd/MM/yyyy") : "",
                    DetectedPlaceId = o.IsFx2F0 ? ((string.Format("Từ F{0} chuyển thành F0", o.FromLevel == 0 ? "x" : o.FromLevel.ToString())) + "; " + (o.DetectedPlaceId.HasValue ? o.DetectedPlace.Name : "")) : (o.DetectedPlaceId.HasValue ? o.DetectedPlace.Name : ""),
                    TreatmentFacilityId = o.TreatmentFacilityId.HasValue ? o.TreatmentFacility.Name : "",
                    EpidemicAreaId = o.EpidemicArea.Name,
                    F0Status = o.IsCured ? "<span class='text-success'>Đã khỏi bệnh</span>" : (o.IsDeath ? "<span class='text-danger'>Đã tử vong</span>" : "Đang điều trị"),
                    o.PhoneNumber,
                    o.YearOfBirth,
                    FxCount =
                       string.Format(@"<span class='text-danger'>{0} F1;</span><br />", o.FCases.Count()) +
                       string.Format(@"<span class='text-primary'>{0} F2;</span><br />", o.FCases.Sum(x => x.FCases.Count())) +
                       string.Format(@"<span class='text-success'>{0} F3</span><br />", o.FCases.Sum(x => x.FCases.Sum(z => z.FCases.Count()))),

                    //CreateDate = o.CreateDate.ToString("dd/MM/yyyy"),
                    //CreateBy = new HtmlHelpersDB(_repository).GetCreateBy(o.CreateBy),

                })
            });
        }
        #endregion
        #region Nhập ca F0
        [Route("fcase/create", Name = "FCaseCreate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.FCase, AppEnum.Covid })]
        public async Task<IActionResult> Create()
        {
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            //ViewData["Province"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", 26);
            ViewData["ProvinceAddr"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", 26);
            var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == 26);
            //ViewData["District"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", 24);
            ViewData["DistrictAddr"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", 24);

            var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == 24);
            ViewData["CommuneAddr"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name");
            //Chỉ lấy các địa phương đã có khu, ổ dịch
            //ViewData["Commune"] = new SelectList(communes.Where(o => o.EpidemicAreas.Any()).OrderBy(o => o.OrdinalNumber), "Id", "Name");

            var detectedPlaces = await _repository.GetRepository<DetectedPlace>().GetAllAsync();
            ViewData["DetectedPlace"] = new SelectList(detectedPlaces.OrderBy(o => o.OrdinalNumber), "Id", "Name");
            var treatmentFacilities = await _repository.GetRepository<TreatmentFacility>().GetAllAsync();
            ViewData["TreatmentFacility"] = new SelectList(treatmentFacilities.OrderBy(o => o.OrdinalNumber), "Id", "Name");

            var epidemicAreas = await _repository.GetRepository<EpidemicArea>().GetAllAsync();
            var epidemicAreaList = epidemicAreas
                .OrderBy(o => o.Commune.District.ProvinceId)
                .ThenBy(o => o.Commune.DistrictId)
                .ThenBy(o => o.CommuneId)
                .ThenByDescending(o => o.OutbreakDate)
                .Select(o => new EpidemicAreaList()
                {
                    Id = o.Id,
                    Name = o.Name + ", " + GetAddressByCommune(o.CommuneId)
                });
            ViewData["EpidemicArea"] = new SelectList(epidemicAreaList, "Id", "Name");

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("fcase/create")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.FCase, AppEnum.Covid })]
        public async Task<IActionResult> Create(FCaseCreate model)
        {
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            //ViewData["Province"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.ProvinceId);
            ViewData["ProvinceAddr"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.AddressProvinceId);

            //var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == model.ProvinceId);
            //ViewData["District"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.DistrictId);

            var districts2 = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == model.AddressProvinceId);
            ViewData["DistrictAddr"] = new SelectList(districts2.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.AddressDistrictId);

            //Chỉ lấy các địa phương đã có khu, ổ dịch
            //var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == model.DistrictId && o.EpidemicAreas.Any());
            //ViewData["Commune"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.CommuneId);

            var communes2 = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == model.AddressDistrictId);
            ViewData["CommuneAddr"] = new SelectList(communes2.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.AddressCommuneId);


            var detectedPlaces = await _repository.GetRepository<DetectedPlace>().GetAllAsync();
            ViewData["DetectedPlace"] = new SelectList(detectedPlaces.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.DetectedPlaceId);
            var treatmentFacilities = await _repository.GetRepository<TreatmentFacility>().GetAllAsync();
            ViewData["TreatmentFacility"] = new SelectList(treatmentFacilities.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.TreatmentFacilityId);

            var epidemicAreas = await _repository.GetRepository<EpidemicArea>().GetAllAsync();
            var epidemicAreaList = epidemicAreas
                .OrderBy(o => o.Commune.District.ProvinceId)
                .ThenBy(o => o.Commune.DistrictId)
                .ThenBy(o => o.CommuneId)
                .ThenByDescending(o => o.OutbreakDate)
                .Select(o => new EpidemicAreaList()
                {
                    Id = o.Id,
                    Name = o.Name + ", " + GetAddressByCommune(o.CommuneId)
                });
            ViewData["EpidemicArea"] = new SelectList(epidemicAreaList, "Id", "Name", model.EpidemicAreaId);

            if (ModelState.IsValid)
            {
                try
                {
                    var strF0Date = StringHelper.KillChars(model.F0Date);
                    DateTime f0Date = DateTime.Now;
                    if (string.IsNullOrEmpty(strF0Date))
                    {
                        ModelState.AddModelError("F0Date", "Vui lòng nhập ngày xác nhận là F0!");
                        return View(model);
                    }
                    if (!string.IsNullOrEmpty(strF0Date))
                    {
                        try
                        {
                            f0Date = DateTime.ParseExact(strF0Date, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    string code = StringHelper.KillChars(model.Code);
                    string name = StringHelper.KillChars(model.Name);
                    string sn = string.Join(".", name.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToArray());
                    string addr = StringHelper.KillChars(model.Address);
                    string mr = StringHelper.KillChars(model.MovingRoute);
                    string ch = StringHelper.KillChars(model.ContactHistory);
                    string phone = StringHelper.KillChars(model.PhoneNumber);
                    string epi = StringHelper.KillChars(model.Epidemiology);

                    var ea = await _repository.GetRepository<EpidemicArea>().ReadAsync(model.EpidemicAreaId);
                    var fCase = new FCase()
                    {
                        Code = code,
                        Name = name,
                        ShortName = sn,
                        Address = addr,
                        YearOfBirth = model.YearOfBirth,
                        PhoneNumber = phone,
                        Epidemiology = epi,
                        AddressCommuneId = model.AddressCommuneId,
                        AddressDistrictId = model.AddressDistrictId,
                        AddressProvinceId = model.AddressProvinceId,
                        MovingRoute = mr,
                        ContactHistory = ch,
                        IsF0 = true,
                        F0Date = f0Date,
                        EpidemicAreaId = model.EpidemicAreaId,
                        EpidemicAreaRelatedId = model.EpidemicAreaRelatedId,
                        DetectedPlaceId = model.DetectedPlaceId,
                        TreatmentFacilityId = model.TreatmentFacilityId,
                        CommuneId = ea.CommuneId,
                        DistrictId = ea.DistrictId,
                        ProvinceId = ea.ProvinceId,
                        CreateBy = AccountId,
                        UpdateBy = AccountId,
                        Level = -1,
                    };

                    int result = await _repository.GetRepository<FCase>().CreateAsync(fCase, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Nhập ca bệnh F0 thành công!";
                        return RedirectToRoute("FCaseIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Nhập ca bệnh F0 không thành công!";
                        ModelState.AddModelError(string.Empty, "Nhập ca bệnh F0 không thành công!");
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
        #region Cập nhật ca F0
        [Route("fcase/update/{id}", Name = "FCaseUpdate")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.FCase, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id)
        {
            var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
            if (fCase == null)
                return RedirectToRoute("FCaseIndex");
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            //ViewData["Province"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", fCase.ProvinceId);
            ViewData["ProvinceAddr"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", fCase.AddressProvinceId);

            //var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == fCase.ProvinceId);
            //ViewData["District"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", fCase.DistrictId);

            var districts2 = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == fCase.AddressProvinceId);
            ViewData["DistrictAddr"] = new SelectList(districts2.OrderBy(o => o.OrdinalNumber), "Id", "Name", fCase.AddressDistrictId);

            //Chỉ lấy các địa phương đã có khu, ổ dịch
            //var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == fCase.DistrictId && o.EpidemicAreas.Any());
            //ViewData["Commune"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name");

            var communes2 = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == fCase.AddressDistrictId);
            ViewData["CommuneAddr"] = new SelectList(communes2.OrderBy(o => o.OrdinalNumber), "Id", "Name");

            var detectedPlaces = await _repository.GetRepository<DetectedPlace>().GetAllAsync();
            ViewData["DetectedPlace"] = new SelectList(detectedPlaces.OrderBy(o => o.OrdinalNumber), "Id", "Name", fCase.DetectedPlaceId);
            var treatmentFacilities = await _repository.GetRepository<TreatmentFacility>().GetAllAsync();
            ViewData["TreatmentFacility"] = new SelectList(treatmentFacilities.OrderBy(o => o.OrdinalNumber), "Id", "Name", fCase.TreatmentFacilityId);

            //var epidemicAreas = await _repository.GetRepository<EpidemicArea>().GetAllAsync(o => o.CommuneId == fCase.CommuneId);
            //ViewData["EpidemicArea"] = new SelectList(epidemicAreas.OrderByDescending(o => o.OutbreakDate), "Id", "Name", fCase.EpidemicAreaId);
            var epidemicAreas = await _repository.GetRepository<EpidemicArea>().GetAllAsync();
            var epidemicAreaList = epidemicAreas
                .OrderBy(o => o.Commune.District.ProvinceId)
                .ThenBy(o => o.Commune.DistrictId)
                .ThenBy(o => o.CommuneId)
                .ThenByDescending(o => o.OutbreakDate)
                .Select(o => new EpidemicAreaList()
                {
                    Id = o.Id,
                    Name = o.Name + ", " + GetAddressByCommune(o.CommuneId)
                });
            ViewData["EpidemicArea"] = new SelectList(epidemicAreaList, "Id", "Name", fCase.EpidemicAreaId);

            var model = new FCaseUpdate()
            {
                Address = fCase.Address,
                AddressCommuneId = fCase.AddressCommuneId,
                AddressDistrictId = fCase.AddressDistrictId,
                AddressProvinceId = fCase.AddressProvinceId,
                Code = fCase.Code,
                ContactHistory = fCase.ContactHistory,
                DetectedPlaceId = fCase.DetectedPlaceId ?? 0,
                EpidemicAreaId = fCase.EpidemicAreaId,
                EpidemicAreaRelatedId = fCase.EpidemicAreaRelatedId,
                F0Date = fCase.F0Date.HasValue ? fCase.F0Date.Value.ToString("dd/MM/yyyy") : "",
                MovingRoute = fCase.MovingRoute,
                Name = fCase.Name,
                TreatmentFacilityId = fCase.TreatmentFacilityId ?? 0,
                Id = fCase.Id,
                PhoneNumber = fCase.PhoneNumber,
                YearOfBirth = fCase.YearOfBirth,
                Epidemiology = fCase.Epidemiology
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("fcase/update/{id}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.FCase, AppEnum.Covid })]
        public async Task<IActionResult> Update(long id, FCaseUpdate model)
        {
            var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
            if (fCase == null || id != model.Id)
                return RedirectToRoute("FCaseIndex");
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            //ViewData["Province"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.ProvinceId);
            ViewData["ProvinceAddr"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.AddressProvinceId);

            //var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == model.ProvinceId);
            //ViewData["District"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.DistrictId);

            var districts2 = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == model.AddressProvinceId);
            ViewData["DistrictAddr"] = new SelectList(districts2.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.AddressDistrictId);

            //Chỉ lấy các địa phương đã có khu, ổ dịch
            //var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == model.DistrictId && o.EpidemicAreas.Any());
            //ViewData["Commune"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.CommuneId);

            var communes2 = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == model.AddressDistrictId);
            ViewData["CommuneAddr"] = new SelectList(communes2.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.AddressCommuneId);

            var detectedPlaces = await _repository.GetRepository<DetectedPlace>().GetAllAsync();
            ViewData["DetectedPlace"] = new SelectList(detectedPlaces.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.DetectedPlaceId);
            var treatmentFacilities = await _repository.GetRepository<TreatmentFacility>().GetAllAsync();
            ViewData["TreatmentFacility"] = new SelectList(treatmentFacilities.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.TreatmentFacilityId);

            //var epidemicAreas = await _repository.GetRepository<EpidemicArea>().GetAllAsync(o => o.CommuneId == model.CommuneId);
            //ViewData["EpidemicArea"] = new SelectList(epidemicAreas.OrderByDescending(o => o.OutbreakDate), "Id", "Name", model.EpidemicAreaId);

            var epidemicAreas = await _repository.GetRepository<EpidemicArea>().GetAllAsync();
            var epidemicAreaList = epidemicAreas
                .OrderBy(o => o.Commune.District.ProvinceId)
                .ThenBy(o => o.Commune.DistrictId)
                .ThenBy(o => o.CommuneId)
                .ThenByDescending(o => o.OutbreakDate)
                .Select(o => new EpidemicAreaList()
                {
                    Id = o.Id,
                    Name = o.Name + ", " + GetAddressByCommune(o.CommuneId)
                });
            ViewData["EpidemicArea"] = new SelectList(epidemicAreaList, "Id", "Name", model.EpidemicAreaId);

            if (ModelState.IsValid)
            {
                try
                {
                    var strF0Date = StringHelper.KillChars(model.F0Date);
                    DateTime f0Date = DateTime.Now;
                    if (string.IsNullOrEmpty(strF0Date))
                    {
                        ModelState.AddModelError("F0Date", "Vui lòng nhập ngày xác nhận là F0!");
                        return View(model);
                    }
                    if (!string.IsNullOrEmpty(strF0Date))
                    {
                        try
                        {
                            f0Date = DateTime.ParseExact(strF0Date, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    string code = StringHelper.KillChars(model.Code);
                    string name = StringHelper.KillChars(model.Name);
                    string sn = string.Join(".", name.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToArray());
                    string addr = StringHelper.KillChars(model.Address);
                    string mr = StringHelper.KillChars(model.MovingRoute);
                    string ch = StringHelper.KillChars(model.ContactHistory);
                    string phone = StringHelper.KillChars(model.PhoneNumber);
                    string epi = StringHelper.KillChars(model.Epidemiology);
                    var ea = await _repository.GetRepository<EpidemicArea>().ReadAsync(model.EpidemicAreaId);
                    fCase.Code = code;
                    fCase.Name = name;
                    fCase.ShortName = sn;
                    fCase.Address = addr;
                    fCase.YearOfBirth = model.YearOfBirth;
                    fCase.PhoneNumber = phone;
                    fCase.Epidemiology = epi;
                    fCase.AddressCommuneId = model.AddressCommuneId;
                    fCase.AddressDistrictId = model.AddressDistrictId;
                    fCase.AddressProvinceId = model.AddressProvinceId;
                    fCase.MovingRoute = mr;
                    fCase.ContactHistory = ch;
                    fCase.F0Date = f0Date;
                    fCase.EpidemicAreaId = model.EpidemicAreaId;
                    fCase.EpidemicAreaRelatedId = model.EpidemicAreaRelatedId;
                    fCase.DetectedPlaceId = model.DetectedPlaceId;
                    fCase.TreatmentFacilityId = model.TreatmentFacilityId;
                    fCase.CommuneId = ea.CommuneId;
                    fCase.DistrictId = ea.DistrictId;
                    fCase.ProvinceId = ea.ProvinceId;
                    fCase.UpdateBy = AccountId;

                    int result = await _repository.GetRepository<FCase>().UpdateAsync(fCase, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Cập nhật ca bệnh F0 thành công!";
                        return RedirectToRoute("FCaseIndex");
                    }
                    else
                    {
                        ViewBag.Error = "Cập nhật ca bệnh F0 không thành công!";
                        ModelState.AddModelError(string.Empty, "Cập nhật ca bệnh F0 không thành công!");
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
        #region Chi tiết ca F0/Hoặc ca Fx
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.FCase, AppEnum.Covid })]
        [Route("fcase/detail/{id}", Name = "FCaseDetail")]
        public async Task<IActionResult> Detail(long id)
        {
            var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
            var additionalInfo = new FCaseAdditionalInfo();
            ViewData["FCaseAdditionalInfo"] = additionalInfo;
            if (fCase.IsF0)
            {
                if (fCase.IsFx2F0)
                {
                    /*
                    if (fCase.F0Id.HasValue && fCase.F0Foretell.HasValue && fCase.F0Id.Value == fCase.F0Foretell.Value)
                    {
                        var f0Id = fCase.F0Id.Value;
                        var f0 = await _repository.GetRepository<FCase>().ReadAsync(f0Id);
                        additionalInfo = new FCaseAdditionalInfo()
                        {
                            F0Foretell = f0Id,
                            F0Id = f0Id,
                            F0ForetellName = f0.Name,
                            F0Name = f0.Name,
                            IsFx2F0 = true,
                            FromLevel = fCase.FromLevel == 0 ? "x" : fCase.FromLevel.ToString(),
                        };
                    }
                    else
                    {
                        additionalInfo.IsFx2F0 = true;
                        additionalInfo.FromLevel = fCase.FromLevel == 0 ? "x" : fCase.FromLevel.ToString();
                        var f0Id = fCase.F0Id;
                        var f0ForetellId = fCase.F0Foretell;
                        if (f0Id.HasValue)
                        {
                            var f0 = await _repository.GetRepository<FCase>().ReadAsync(f0Id.Value);
                            if (f0 != null)
                            {
                                additionalInfo.F0Id = f0Id.Value;
                                additionalInfo.F0Name = f0.Name;
                            }
                        }
                        if (f0ForetellId.HasValue)
                        {
                            var f0Foretell = await _repository.GetRepository<FCase>().ReadAsync(f0ForetellId.Value);
                            if (f0Foretell != null)
                            {
                                additionalInfo.F0Id = f0ForetellId.Value;
                                additionalInfo.F0Name = f0Foretell.Name;
                            }
                        }
                    }
                    */
                    additionalInfo.IsFx2F0 = true;
                    additionalInfo.FromLevel = fCase.LevelInitial.ToString();
                    if (fCase.F0Foretell.Value == fCase.F0Id.Value)
                    {
                        var f0 = await _repository.GetRepository<FCase>().ReadAsync(fCase.F0Id.Value);
                        additionalInfo = new FCaseAdditionalInfo()
                        {
                            F0Foretell = fCase.F0Id.Value,
                            F0Id = fCase.F0Id.Value,
                            F0ForetellName = f0.Name,
                            F0Name = f0.Name,
                        };
                    }
                    else
                    {

                        var f0 = await _repository.GetRepository<FCase>().ReadAsync(fCase.F0Id.Value);
                        if (f0 != null)
                        {
                            additionalInfo.F0Id = fCase.F0Id.Value;
                            additionalInfo.F0Name = f0.Name;
                        }
                        var f0Foretell = await _repository.GetRepository<FCase>().ReadAsync(fCase.F0Foretell.Value);
                        if (f0Foretell != null)
                        {
                            additionalInfo.F0Foretell = fCase.F0Foretell.Value;
                            additionalInfo.F0ForetellName = f0Foretell.Name;
                        }
                    }
                    ViewData["FCaseAdditionalInfo"] = additionalInfo;
                }
                return View("Detail", fCase);
            }
            if (!fCase.IsF0 && fCase.Level == 1)
                return View("DetailF1", fCase);
            if (!fCase.IsF0 && fCase.Level == 2)
                return View("DetailF2", fCase);
            if (!fCase.IsF0 && fCase.Level == 3)
                return View("DetailF3", fCase);
            return View("DetailFx", fCase);
        }
        #endregion

        #region Xác nhận khỏi bệnh/tử vong
        [Route("fcase/confirm/{id}/{type}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.FCase, AppEnum.Covid })]
        public async Task<IActionResult> ConfirmPartial(long id, int type)
        {
            var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
            if (fCase == null)
            {
                return View("_ModalNotFound", "Không tìm thấy nội dung quý vị yêu cầu!");
            }

            var model = new FCaseConfirm()
            {
                Id = id,
                Type = type,
            };
            return PartialView("_ConfirmPartial", model);
        }

        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.FCase, AppEnum.Covid })]
        [Route("fcase/confirm-post")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(FCaseConfirm model)
        {
            try
            {
                var strDate = StringHelper.KillChars(model.Date);
                if (string.IsNullOrEmpty(strDate))
                {
                    return Json(new { success = false, message = "Vui lòng nhập ngày!" });
                }
                var fCase = await _repository.GetRepository<FCase>().ReadAsync(model.Id);
                if (fCase == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin F0!" });
                }
                DateTime? date = null;
                try
                {
                    date = DateTime.ParseExact(strDate, "dd/MM/yyyy", null);
                }
                catch { }
                if (date == null)
                {
                    return Json(new { success = false, message = "Vui lòng nhập ngày!" });
                }
                //Kiểm tra nếu ngày trước ngày phát hiện hoặc sau ngày hiện tại thì báo lỗi
                if (date.Value.Date > DateTime.Now.Date)
                {
                    return Json(new { success = false, message = "Ngày không được phép sau ngày hiện tại!" });
                }
                if (date.Value.Date < fCase.F0Date.Value.Date)
                {
                    return Json(new { success = false, message = "Ngày không được phép trước ngày xác nhận F0!" });
                }
                if (model.Type == 1)
                {
                    fCase.IsCured = true;
                    fCase.CuredDate = date;
                }
                if (model.Type == 0)
                {
                    fCase.IsDeath = true;
                    fCase.DeathDate = date;
                }
                int result = await _repository.GetRepository<FCase>().UpdateAsync(fCase, AccountId);
                if (result > 0)
                {
                    return Json(new { success = true, message = "Xác nhận thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Xác nhận không thành công!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Xác nhận không thành công! Lỗi: " + ex.Message });
            }
        }
        #endregion
       
        #region Nhập kết quả xét nghiệm điều trị
        [Route("fcase/test-result/{id}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.FCaseTestResult, AppEnum.Covid })]
        public async Task<IActionResult> TestResultPartial(long id)
        {
            var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
            if (fCase == null)
            {
                return View("_ModalNotFound", "Không tìm thấy nội dung quý vị yêu cầu!");
            }

            var model = new FCaseTestResult()
            {
                Id = id,
            };
            return PartialView("_TestResultPartial", model);
        }

        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.FCaseTestResult, AppEnum.Covid })]
        [Route("fcase/test-result-post")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TestResult(FCaseTestResult model)
        {
            try
            {
                var fCase = await _repository.GetRepository<FCase>().ReadAsync(model.Id);
                if (fCase == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin F0!" });
                }
                var strTestDate = StringHelper.KillChars(model.TestDate);
                if (string.IsNullOrEmpty(strTestDate))
                {
                    return Json(new { success = false, message = "Vui lòng nhập ngày lấy mẫu!" });
                }
                var strResultDate = StringHelper.KillChars(model.ResultDate);
                if (string.IsNullOrEmpty(strResultDate))
                {
                    return Json(new { success = false, message = "Vui lòng nhập ngày trả kết quả!" });
                }

                DateTime testDate = DateTime.Now;
                DateTime resultDate = DateTime.Now;
                try
                {
                    testDate = DateTime.ParseExact(strTestDate, "dd/MM/yyyy", null);
                    resultDate = DateTime.ParseExact(strResultDate, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Nhập kết quả không thành công! Lỗi: " + ex.Message });
                }

                //Kiểm tra nếu sau ngày hiện tại thì báo lỗi
                if (testDate.Date > DateTime.Now.Date)
                {
                    return Json(new { success = false, message = "Ngày không được phép sau ngày hiện tại!" });
                }
                //Kiểm tra nếu sau ngày hiện tại thì báo lỗi
                if (resultDate.Date > DateTime.Now.Date)
                {
                    return Json(new { success = false, message = "Ngày không được phép sau ngày hiện tại!" });
                }
                var resultDetail = StringHelper.KillChars(model.ResultDetail);
                var temperature = StringHelper.KillChars(model.Temperature);
                var testResult = new TestResult()
                {
                    FCaseId = model.Id,
                    TestDate = testDate,
                    ResultDate = resultDate,
                    ResultDetail = resultDetail,
                    Temperature = temperature,
                    CreateBy = AccountId,
                    UpdateBy = AccountId,
                    IsPositive = model.IsPositive
                };
                int result = await _repository.GetRepository<TestResult>().CreateAsync(testResult, AccountId);
                if (result > 0)
                {
                    return Json(new { success = true, message = "Nhập kết quả thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Nhập kết quả không thành công!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Nhập kết quả không thành công! Lỗi: " + ex.Message });
            }
        }
        #endregion
        #region Xóa kết quả xét nghiệm
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.FCaseTestResult, AppEnum.Covid })]
        [Route("fcase/test-result-delete/{id}")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTestResult(long id)
        {
            try
            {
                var testResult = await _repository.GetRepository<TestResult>().ReadAsync(id);
                if (testResult == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin kết quả!" });
                }
                int result = await _repository.GetRepository<TestResult>().DeleteAsync(testResult, AccountId);
                if (result > 0)
                {
                    return Json(new { success = true, message = "Xóa kết quả thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Xóa kết quả không thành công!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Xóa kết quả không thành công! Lỗi: " + ex.Message });
            }
        }
        #endregion
        #region Cập nhật xét nghiệm điều trị
        [Route("fcase/test-result-update/{id}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.FCaseTestResult, AppEnum.Covid })]
        public async Task<IActionResult> TestResultUpdatePartial(long id)
        {
            var testResult = await _repository.GetRepository<TestResult>().ReadAsync(id);
            if (testResult == null)
            {
                return View("_ModalNotFound", "Không tìm thấy nội dung quý vị yêu cầu!");
            }

            var model = new FCaseTestResultUpdate()
            {
                Id = id,
                IsPositive = testResult.IsPositive,
                ResultDate = testResult.ResultDate.ToString("dd/MM/yyyy"),
                ResultDetail = testResult.ResultDetail,
                TestDate = testResult.TestDate.ToString("dd/MM/yyyy"),
                Temperature = testResult.Temperature,
            };
            return PartialView("_TestResultUpdatePartial", model);
        }

        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.FCaseTestResult, AppEnum.Covid })]
        [Route("fcase/test-result-update-post")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TestResultUpdate(FCaseTestResultUpdate model)
        {
            try
            {
                var testResult = await _repository.GetRepository<TestResult>().ReadAsync(model.Id);
                if (testResult == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin F0!" });
                }
                var strTestDate = StringHelper.KillChars(model.TestDate);
                if (string.IsNullOrEmpty(strTestDate))
                {
                    return Json(new { success = false, message = "Vui lòng nhập ngày lấy mẫu!" });
                }
                var strResultDate = StringHelper.KillChars(model.ResultDate);
                if (string.IsNullOrEmpty(strResultDate))
                {
                    return Json(new { success = false, message = "Vui lòng nhập ngày trả kết quả!" });
                }

                DateTime testDate = DateTime.Now;
                DateTime resultDate = DateTime.Now;
                try
                {
                    testDate = DateTime.ParseExact(strTestDate, "dd/MM/yyyy", null);
                    resultDate = DateTime.ParseExact(strResultDate, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Nhập kết quả không thành công! Lỗi: " + ex.Message });
                }

                //Kiểm tra nếu sau ngày hiện tại thì báo lỗi
                if (testDate.Date > DateTime.Now.Date)
                {
                    return Json(new { success = false, message = "Ngày không được phép sau ngày hiện tại!" });
                }
                //Kiểm tra nếu sau ngày hiện tại thì báo lỗi
                if (resultDate.Date > DateTime.Now.Date)
                {
                    return Json(new { success = false, message = "Ngày không được phép sau ngày hiện tại!" });
                }
                var resultDetail = StringHelper.KillChars(model.ResultDetail);
                var temperature = StringHelper.KillChars(model.Temperature);
                testResult.TestDate = testDate;
                testResult.ResultDate = resultDate;
                testResult.ResultDetail = resultDetail;
                testResult.Temperature = temperature;
                testResult.UpdateBy = AccountId;
                testResult.IsPositive = model.IsPositive;

                int result = await _repository.GetRepository<TestResult>().UpdateAsync(testResult, AccountId);
                if (result > 0)
                {
                    return Json(new { success = true, message = "Cập nhật kết quả thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Cập nhật kết quả không thành công!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Cập nhật kết quả không thành công! Lỗi: " + ex.Message });
            }
        }
        #endregion

        #region Nhập trường hợp Fx
        [Route("fcase/create-fx/{id}", Name = "FCaseCreateFx")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.FCaseFx, AppEnum.Covid })]
        public async Task<IActionResult> CreateFx(long id)
        {
            var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
            if (fCase == null)
            {
                TempData["Error"] = "Không tìm thấy nội dung bạn yêu cầu!";
                return RedirectToRoute("FCaseIndex");
            }
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            ViewData["ProvinceAddr"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", 26);
            var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == 26);
            ViewData["DistrictAddr"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", 24);
            var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == 24);
            ViewData["CommuneAddr"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name");

            var quarantinePlaces = await _repository.GetRepository<QuarantinePlace>().GetAllAsync();
            ViewData["QuarantinePlace"] = new SelectList(quarantinePlaces.OrderByDescending(o => o.OpenDate), "Id", "Name");
            var quarantineTypes = await _repository.GetRepository<QuarantineType>().GetAllAsync();
            ViewData["QuarantineType"] = new SelectList(quarantineTypes.OrderBy(o => o.OrdinalNumber), "Id", "Name");

            var riskClassifications = await _repository.GetRepository<RiskClassification>().GetAllAsync();
            ViewData["RiskClassification"] = new SelectList(riskClassifications.OrderBy(o => o.OrdinalNumber), "Id", "Name");

            var model = new FxCaseCreate()
            {
                FCaseId = id,
                ParrentName = fCase.Name,
                NextLevel = fCase.Level == -1 ? 1 : fCase.Level + 1
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("fcase/create-fx/{id}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.FCaseFx, AppEnum.Covid })]
        public async Task<IActionResult> CreateFx(long id, FxCaseCreate model)
        {
            var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
            if (fCase == null || (id != model.FCaseId))
            {
                TempData["Error"] = "Không tìm thấy nội dung bạn yêu cầu!";
                return RedirectToRoute("FCaseIndex");
            }
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            ViewData["ProvinceAddr"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.AddressProvinceId);
            var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == model.AddressProvinceId);
            ViewData["DistrictAddr"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.AddressDistrictId);
            var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == model.AddressDistrictId);
            ViewData["CommuneAddr"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.AddressCommuneId);

            var quarantinePlaces = await _repository.GetRepository<QuarantinePlace>().GetAllAsync();
            ViewData["QuarantinePlace"] = new SelectList(quarantinePlaces.OrderByDescending(o => o.OpenDate), "Id", "Name", model.QuarantinePlaceId);
            var quarantineTypes = await _repository.GetRepository<QuarantineType>().GetAllAsync();
            ViewData["QuarantineType"] = new SelectList(quarantineTypes.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.QuarantineTypeId);

            var riskClassifications = await _repository.GetRepository<RiskClassification>().GetAllAsync();
            ViewData["RiskClassification"] = new SelectList(riskClassifications.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.RiskClassificationId);

            if (ModelState.IsValid)
            {
                try
                {
                    var strStartDate = StringHelper.KillChars(model.MonitorStartDate);
                    var strEndDate = StringHelper.KillChars(model.MonitorEndDate);
                    var strFxContactDate = StringHelper.KillChars(model.FxContactDate);
                    var strTracingDate = StringHelper.KillChars(model.TracingDate);
                    DateTime? startDate = null;
                    DateTime? endDate = null;
                    DateTime? fxContactDate = null;
                    DateTime? tracingDate = null;

                    if (!string.IsNullOrEmpty(strStartDate))
                    {
                        try
                        {
                            startDate = DateTime.ParseExact(strStartDate, "dd/MM/yyyy", null);
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
                    if (!string.IsNullOrEmpty(strFxContactDate))
                    {
                        try
                        {
                            fxContactDate = DateTime.ParseExact(strFxContactDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    if (!string.IsNullOrEmpty(strTracingDate))
                    {
                        try
                        {
                            tracingDate = DateTime.ParseExact(strTracingDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    if (startDate.HasValue && endDate.HasValue && endDate.Value.Date < startDate.Value.Date)
                    {
                        ViewBag.Error = "Ngày bắt đầu không được sau ngày kết thúc!";
                        ModelState.AddModelError(string.Empty, "Ngày bắt đầu không được sau ngày kết thúc!");
                        return View(model);
                    }
                    string name = StringHelper.KillChars(model.Name);
                    string sn = string.Join(".", name.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToArray());
                    string addr = StringHelper.KillChars(model.Address);
                    string mr = StringHelper.KillChars(model.MovingRoute);
                    string ch = StringHelper.KillChars(model.ContactHistory);
                    string phone = StringHelper.KillChars(model.PhoneNumber);
                    string epi = StringHelper.KillChars(model.Epidemiology);

                    int level = fCase.Level == -1 ? 1 : fCase.Level + 1;
                    var fxCase = new FCase()
                    {
                        FCaseId = id,
                        Name = name,
                        ShortName = sn,
                        Address = addr,
                        YearOfBirth = model.YearOfBirth,
                        PhoneNumber = model.PhoneNumber,
                        Epidemiology = epi,
                        AddressCommuneId = model.AddressCommuneId,
                        AddressDistrictId = model.AddressDistrictId,
                        AddressProvinceId = model.AddressProvinceId,
                        MovingRoute = mr,
                        ContactHistory = ch,
                        MonitorStartDate = startDate,
                        MonitorEndDate = endDate,
                        FxContactDate = fxContactDate,
                        QuarantineTypeId = model.QuarantineTypeId,
                        QuarantinePlaceId = model.QuarantinePlaceId,
                        QuarantineDays = model.QuarantineDays,
                        RiskClassificationId = model.RiskClassificationId ?? 4,//Chọn vào Chưa đánh giá nếu chưa được chọn
                        EpidemicAreaId = fCase.EpidemicAreaId,
                        CommuneId = fCase.CommuneId,
                        DistrictId = fCase.DistrictId,
                        ProvinceId = fCase.ProvinceId,
                        CreateBy = AccountId,
                        UpdateBy = AccountId,
                        Level = level,
                        LevelInitial = level,
                        TracingDate = tracingDate,
                    };

                    int result = await _repository.GetRepository<FCase>().CreateAsync(fxCase, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = string.Format(@"Nhập trường hợp F{0} thành công!", level);
                        //return RedirectToRoute("FCaseIndex");
                        return new RedirectResult(Url.RouteUrl("FCaseDetail", new { id }) + "#tab_5_4");
                    }
                    else
                    {
                        ViewBag.Error = "Nhập không thành công!";
                        ModelState.AddModelError(string.Empty, "Nhập không thành công!");
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
        #region Xóa trường hợp Fx
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.FCaseFx, AppEnum.Covid })]
        [Route("fcase/delete-fx/{id}")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFx(long id)
        {
            try
            {
                var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
                if (fCase == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin!" });
                }
                var any = await _repository.GetRepository<FCase>().AnyAsync(o => o.FCaseId == id);
                if (any)
                {
                    return Json(new { success = false, message = "Không xóa được vì có các F liên quan!" });
                }
                //Xóa tài liệu liên quan nếu có.
                var fds = await _repository.GetRepository<FCaseDocument>().GetAllAsync(o => o.FCaseId == id);
                if (fds.Any())
                {
                    foreach (var item in fds)
                    {
                        string file = item.DocumentPath;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + file);
                        if (System.IO.File.Exists(filePath))
                        {
                            var attr = System.IO.File.GetAttributes(filePath);
                            if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            {
                                System.IO.File.SetAttributes(filePath, attr ^ FileAttributes.ReadOnly);
                            }
                            try
                            {
                                System.IO.File.Delete(filePath);
                            }
                            catch { }
                        }
                    }
                }
                int result1 = await _repository.GetRepository<FCaseDocument>().DeleteAsync(o => o.FCaseId == id, AccountId);
                //Xóa các kết quả xét nghiệm nếu có
                int result2 = await _repository.GetRepository<TestResult>().DeleteAsync(o => o.FCaseId == id, AccountId);
                int result = await _repository.GetRepository<FCase>().DeleteAsync(fCase, AccountId);
                if (result > 0)
                {
                    return Json(new { success = true, message = "Xóa thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Xóa không thành công!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Xóa không thành công! Lỗi: " + ex.Message });
            }
        }
        #endregion
        #region Cập nhật trường hợp Fx
        [Route("fcase/update-fx/{id}/{fxId}", Name = "FCaseUpdateFx")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.FCaseFx, AppEnum.Covid })]
        public async Task<IActionResult> UpdateFx(long id, long fxId)
        {
            var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
            if (fCase == null)
            {
                TempData["Error"] = "Không tìm thấy nội dung bạn yêu cầu!";
                return RedirectToRoute("FCaseIndex");
            }
            var fx = await _repository.GetRepository<FCase>().ReadAsync(fxId);
            if (fx == null)
            {
                TempData["Error"] = "Không tìm thấy nội dung bạn yêu cầu!";
                return RedirectToRoute("FCaseIndex");
            }
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            ViewData["ProvinceAddr"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", fx.AddressProvinceId);
            var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == fx.AddressProvinceId);
            ViewData["DistrictAddr"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", fx.AddressDistrictId);
            var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == fx.AddressDistrictId);
            ViewData["CommuneAddr"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name", fx.AddressCommuneId);

            var quarantinePlaces = await _repository.GetRepository<QuarantinePlace>().GetAllAsync();
            ViewData["QuarantinePlace"] = new SelectList(quarantinePlaces.OrderByDescending(o => o.OpenDate), "Id", "Name", fx.QuarantinePlaceId);
            var quarantineTypes = await _repository.GetRepository<QuarantineType>().GetAllAsync();
            ViewData["QuarantineType"] = new SelectList(quarantineTypes.OrderBy(o => o.OrdinalNumber), "Id", "Name", fx.QuarantineTypeId);

            var riskClassifications = await _repository.GetRepository<RiskClassification>().GetAllAsync();
            ViewData["RiskClassification"] = new SelectList(riskClassifications.OrderBy(o => o.OrdinalNumber), "Id", "Name", fx.RiskClassificationId);

            var model = new FxCaseUpdate()
            {
                FCaseId = id,
                FxCaseId = fxId,
                ParrentName = fCase.Name,
                NextLevel = fCase.Level == -1 ? 1 : fCase.Level + 1,
                Address = fx.Address,
                AddressCommuneId = fx.AddressCommuneId,
                AddressDistrictId = fx.AddressDistrictId,
                AddressProvinceId = fx.AddressProvinceId,
                RiskClassificationId = fx.RiskClassificationId,
                ContactHistory = fx.ContactHistory,
                MonitorEndDate = fx.MonitorEndDate.HasValue ? fx.MonitorEndDate.Value.ToString("dd/MM/yyyy") : "",
                MonitorStartDate = fx.MonitorStartDate.HasValue ? fx.MonitorStartDate.Value.ToString("dd/MM/yyyy") : "",
                MovingRoute = fx.MovingRoute,
                Name = fx.Name,
                QuarantineDays = fx.QuarantineDays,
                QuarantinePlaceId = fx.QuarantinePlaceId,
                QuarantineTypeId = fx.QuarantineTypeId,
                PhoneNumber = fx.PhoneNumber,
                YearOfBirth = fx.YearOfBirth,
                Epidemiology = fx.Epidemiology,
                FxContactDate = fx.FxContactDate.HasValue ? fx.FxContactDate.Value.ToString("dd/MM/yyyy") : "",
                TracingDate = fx.TracingDate.HasValue ? fx.TracingDate.Value.ToString("dd/MM/yyyy") : "",
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("fcase/update-fx/{id}/{fxId}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.FCaseFx, AppEnum.Covid })]
        public async Task<IActionResult> UpdateFx(long id, long fxId, FxCaseUpdate model)
        {
            var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
            if (fCase == null || (id != model.FCaseId))
            {
                TempData["Error"] = "Không tìm thấy nội dung bạn yêu cầu!";
                return RedirectToRoute("FCaseIndex");
            }
            var fx = await _repository.GetRepository<FCase>().ReadAsync(fxId);
            if (fx == null || (fxId != model.FxCaseId))
            {
                TempData["Error"] = "Không tìm thấy nội dung bạn yêu cầu!";
                return RedirectToRoute("FCaseIndex");
            }
            var provinces = await _repository.GetRepository<Province>().GetAllAsync();
            ViewData["ProvinceAddr"] = new SelectList(provinces.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.AddressProvinceId);
            var districts = await _repository.GetRepository<District>().GetAllAsync(o => o.ProvinceId == model.AddressProvinceId);
            ViewData["DistrictAddr"] = new SelectList(districts.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.AddressDistrictId);
            var communes = await _repository.GetRepository<Commune>().GetAllAsync(o => o.DistrictId == model.AddressDistrictId);
            ViewData["CommuneAddr"] = new SelectList(communes.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.AddressCommuneId);

            var quarantinePlaces = await _repository.GetRepository<QuarantinePlace>().GetAllAsync();
            ViewData["QuarantinePlace"] = new SelectList(quarantinePlaces.OrderByDescending(o => o.OpenDate), "Id", "Name", model.QuarantinePlaceId);
            var quarantineTypes = await _repository.GetRepository<QuarantineType>().GetAllAsync();
            ViewData["QuarantineType"] = new SelectList(quarantineTypes.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.QuarantineTypeId);

            var riskClassifications = await _repository.GetRepository<RiskClassification>().GetAllAsync();
            ViewData["RiskClassification"] = new SelectList(riskClassifications.OrderBy(o => o.OrdinalNumber), "Id", "Name", model.RiskClassificationId);

            if (ModelState.IsValid)
            {
                try
                {
                    var strStartDate = StringHelper.KillChars(model.MonitorStartDate);
                    var strEndDate = StringHelper.KillChars(model.MonitorEndDate);
                    DateTime? startDate = null;
                    DateTime? endDate = null;
                    var strFxContactDate = StringHelper.KillChars(model.FxContactDate);
                    DateTime? fxContactDate = null;
                    var strTracingDate = StringHelper.KillChars(model.TracingDate);
                    DateTime? tracingDate = null;
                    if (!string.IsNullOrEmpty(strStartDate))
                    {
                        try
                        {
                            startDate = DateTime.ParseExact(strStartDate, "dd/MM/yyyy", null);
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
                    if (!string.IsNullOrEmpty(strFxContactDate))
                    {
                        try
                        {
                            fxContactDate = DateTime.ParseExact(strFxContactDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    if (!string.IsNullOrEmpty(strTracingDate))
                    {
                        try
                        {
                            tracingDate = DateTime.ParseExact(strTracingDate, "dd/MM/yyyy", null);
                        }
                        catch { }
                    }
                    if (startDate.HasValue && endDate.HasValue && endDate.Value.Date < startDate.Value.Date)
                    {
                        ViewBag.Error = "Ngày bắt đầu không được sau ngày kết thúc!";
                        ModelState.AddModelError(string.Empty, "Ngày bắt đầu không được sau ngày kết thúc!");
                        return View(model);
                    }
                    string name = StringHelper.KillChars(model.Name);
                    string sn = string.Join(".", name.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToArray());
                    string addr = StringHelper.KillChars(model.Address);
                    string mr = StringHelper.KillChars(model.MovingRoute);
                    string ch = StringHelper.KillChars(model.ContactHistory);
                    string phone = StringHelper.KillChars(model.PhoneNumber);
                    string epi = StringHelper.KillChars(model.Epidemiology);
                    int level = fCase.Level == -1 ? 1 : fCase.Level + 1;

                    fx.Name = name;
                    fx.ShortName = sn;
                    fx.Address = addr;
                    fx.YearOfBirth = model.YearOfBirth;
                    fx.PhoneNumber = phone;
                    fx.Epidemiology = epi;
                    fx.AddressCommuneId = model.AddressCommuneId;
                    fx.AddressDistrictId = model.AddressDistrictId;
                    fx.AddressProvinceId = model.AddressProvinceId;
                    fx.MovingRoute = mr;
                    fx.ContactHistory = ch;
                    fx.MonitorStartDate = startDate;
                    fx.MonitorEndDate = endDate;
                    fx.QuarantineTypeId = model.QuarantineTypeId;
                    fx.QuarantinePlaceId = model.QuarantinePlaceId;
                    fx.RiskClassificationId = model.RiskClassificationId;
                    fx.QuarantineDays = model.QuarantineDays;
                    fx.UpdateBy = AccountId;
                    fx.FxContactDate = fxContactDate;
                    fx.TracingDate = tracingDate;

                    int result = await _repository.GetRepository<FCase>().UpdateAsync(fx, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = string.Format(@"Cập nhật trường hợp F{0} thành công!", level);
                        //return RedirectToRoute("FCaseIndex");
                        return new RedirectResult(Url.RouteUrl("FCaseDetail", new { id }) + "#tab_5_4");
                    }
                    else
                    {
                        ViewBag.Error = "Cập nhật không thành công!";
                        ModelState.AddModelError(string.Empty, "Cập nhật không thành công!");
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

        #region Xác nhận trường hợp Fx đã thành F0
        [Route("fcase/confirm-f0/{id}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.FCase, AppEnum.Covid })]
        public async Task<IActionResult> ConfirmF0Partial(long id)
        {
            var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
            if (fCase == null)
            {
                return View("_ModalNotFound", "Không tìm thấy nội dung quý vị yêu cầu!");
            }

            var model = new FCaseConfirm()
            {
                Id = id,
                Type = -1,
            };
            return PartialView("_ConfirmF0Partial", model);
        }

        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Update, ModuleEnum.FCase, AppEnum.Covid })]
        [Route("fcase/confirm-f0-post")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmF0(FCaseConfirm model)
        {
            try
            {
                var strDate = StringHelper.KillChars(model.Date);
                if (string.IsNullOrEmpty(strDate))
                {
                    return Json(new { success = false, message = "Vui lòng nhập ngày!" });
                }
                var fCase = await _repository.GetRepository<FCase>().ReadAsync(model.Id);
                if (fCase == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin Fx!" });
                }
                DateTime? date = null;
                try
                {
                    date = DateTime.ParseExact(strDate, "dd/MM/yyyy", null);
                }
                catch { }
                if (date == null)
                {
                    return Json(new { success = false, message = "Vui lòng nhập ngày!" });
                }
                //Kiểm tra nếu sau ngày hiện tại thì báo lỗi
                if (date.Value.Date > DateTime.Now.Date)
                {
                    return Json(new { success = false, message = "Ngày không được phép sau ngày hiện tại!" });
                }

                if (model.Type != -1)
                {//Kiểm tra nếu bị hack
                    return Json(new { success = false, message = "Không thực hiện được!" });
                }
                int level = fCase.Level;
                //long? f0Id = level == 1 ? fCase.FCaseId : (level == 2 ? fCase.ParentFCase.FCaseId : fCase.ParentFCase.ParentFCase.FCaseId);
                long f0Id = await GetF0Id(model.Id);
                long f0ForetellId = await GetF0ForetellId(model.Id);
                /*
                if (level == 1)
                {
                    //Nếu ông này đang là F1, thì F0 chính là ông bố
                    f0Id = fCase.FCaseId;
                    f0Foretell = fCase.FCaseId;
                }
                if (level == 2)
                {
                    f0Foretell = fCase.ParentFCase.FCaseId;
                    //Nếu ông này đang là F2, thì kiểm tra ông bố có phải là F0 hay không
                    if (fCase.ParentFCase.IsF0)
                    {
                        //Nếu ông bố là F0 thì ông này chuyển F0 từ ông bố
                        f0Id = fCase.ParentFCase.Id;
                    }
                    else
                    {
                        //Ngược lại thì lây từ ông nội
                        f0Id = fCase.ParentFCase.FCaseId;
                    }
                }
                if (level == 3)
                {
                    f0Foretell = fCase.ParentFCase.ParentFCase.FCaseId;
                    //Nếu ông này đang là F3, thì kiểm tra ông bố có phải là F0 hay không
                    if (fCase.ParentFCase.IsF0)
                    {
                        //Nếu ông bố là F0 thì ông này chuyển F0 từ ông bố
                        f0Id = fCase.ParentFCase.Id;
                    }
                    else
                    {
                        //Kiểm tra có phải lây từ ông nội hay không
                        if (fCase.ParentFCase.ParentFCase.IsF0)
                        {
                            f0Id = fCase.ParentFCase.ParentFCase.Id;
                        }
                        else
                        {
                            //Ngược lại thì lây từ ông cố
                            f0Id = fCase.ParentFCase.ParentFCase.FCaseId;
                        }
                    }
                }
                */
                var f0Foretell = await _repository.GetRepository<FCase>().ReadAsync(f0ForetellId);

                fCase.EpidemicAreaId = f0Foretell.EpidemicAreaId;
                fCase.CommuneId = f0Foretell.CommuneId;
                fCase.DistrictId = f0Foretell.DistrictId;
                fCase.ProvinceId = f0Foretell.ProvinceId;
                fCase.IsF0 = true;
                fCase.F0Date = date;
                fCase.IsFx2F0 = true;
                fCase.Level = -1;
                fCase.F0Id = f0Id;
                fCase.F0Foretell = f0ForetellId;
                fCase.FromLevel = level;

                int result = await _repository.GetRepository<FCase>().UpdateAsync(fCase, AccountId);
                if (result > 0)
                {
                    //Cập nhật các F cấp dưới, đang quản lý đến F3,
                    //nên nếu đây đang là trường hợp F3 thì nó đang là mức cuối cùng, không có mức con để cập nhật
                    if (level < 3)
                    {
                        var fcases = _repository.GetRepository<FCase>().GetAll(o => o.FCaseId == model.Id).ToList();
                        if (fcases.Any())
                        {
                            foreach (var item in fcases)
                            {
                                var fcases2 = _repository.GetRepository<FCase>().GetAll(o => o.FCaseId == item.Id).ToList();
                                if (fcases2.Any())
                                {
                                    foreach (var item2 in fcases2)
                                    {
                                        item2.Level--;
                                        _repository.GetRepository<FCase>().Update(item2, AccountId);
                                    }
                                }
                                item.Level--;
                                _repository.GetRepository<FCase>().Update(item, AccountId);
                            }
                        }
                    }
                    return Json(new { success = true, message = "Xác nhận thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Xác nhận không thành công!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Xác nhận không thành công! Lỗi: " + ex.Message });
            }
        }
        //Lấy F0 trực tiếp của Fx
        // private async Task<List<OrgChart>> GetFCase(long id)
        private async Task<long> GetF0Id(long id)
        {
            long f0Id = 0;
            var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
            if (fCase.ParentFCase != null && fCase.ParentFCase.IsF0)
                f0Id = fCase.FCaseId.Value;
            else
            {
                if (fCase.ParentFCase != null)
                    f0Id = await GetF0Id(fCase.FCaseId.Value);
            }
            return f0Id;
        }
        //Lấy F0 chỉ điểm của Fx
        // private async Task<List<OrgChart>> GetFCase(long id)
        private async Task<long> GetF0ForetellId(long id)
        {
            long f0Id = 0;
            var fCase = await _repository.GetRepository<FCase>().ReadAsync(id);
            if (fCase.ParentFCase != null && fCase.ParentFCase.IsF0 && !fCase.ParentFCase.IsFx2F0)
                f0Id = fCase.FCaseId.Value;
            else
            {
                if (fCase.ParentFCase != null)
                    f0Id = await GetF0ForetellId(fCase.FCaseId.Value);
            }
            return f0Id;
        }
        #endregion


        #region GetAddressByCommune
        private string GetAddressByCommune(long id, bool shortName = true)
        {
            string addr = "";
            if (id == 0)
                addr = "";
            try
            {
                var commune = _repository.GetRepository<Commune>().Read(id);
                if (commune != null)
                    if (shortName)
                    {
                        var provinceName = commune.District.Province.Name.Replace("tỉnh", "", StringComparison.CurrentCultureIgnoreCase).Replace("thành phố", "", StringComparison.CurrentCultureIgnoreCase);
                        var sn = string.Join(".", provinceName.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToArray());
                        addr = string.Format(@"{0}, {1}, {2}",
                            commune.Name.Replace("thị trấn", "TT", StringComparison.CurrentCultureIgnoreCase).Replace("xã", "", StringComparison.CurrentCultureIgnoreCase).Replace("phường", "", StringComparison.CurrentCultureIgnoreCase),
                            commune.District.Name.Replace("thị xã", "TX", StringComparison.CurrentCultureIgnoreCase).Replace("huyện", "", StringComparison.CurrentCultureIgnoreCase).Replace("quận", "", StringComparison.CurrentCultureIgnoreCase).Replace("thành phố", "", StringComparison.CurrentCultureIgnoreCase),
                            sn
                            );
                    }
                    else
                        addr = string.Format(@"{0}, {1}, {2}", commune.Name, commune.District.Name, commune.District.Province.Name);
            }
            catch { }
            return addr;
        }
        #endregion
    }
}
