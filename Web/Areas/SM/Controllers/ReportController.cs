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
    public class ReportController : BaseController
    {
        #region General
        public ReportController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache) { }
        #endregion
        #region Báo cáo số ca covid
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.FCaseReport, AppEnum.Covid })]
        [Route("bao-cao/so-ca-codvid", Name = "ReportFCase")]
        public async Task<IActionResult> FCase()
        {
            var epidemicAreas = await _repository.GetRepository<EpidemicArea>().GetAllAsync();

            var abc = await _repository.GetRepository<FCase>().GetAllAsync();
            var model = new List<Report1>();

            if (epidemicAreas != null && epidemicAreas.Any())
            {
                model = epidemicAreas.Select(o => new Report1()
                {
                    OutbreakDate = o.OutbreakDate,
                    EpidemicAreaName = o.Name,
                    TotalF0 = o.FCases.Count(o => o.IsF0).ToString("#,##"),
                    IsCured = o.FCases.Count(o => o.IsF0 && o.IsCured).ToString("#,##"),
                    UnderTreatment = o.FCases.Count(o => o.IsF0 && !o.IsCured && !o.IsDeath).ToString("#,##"),
                    IsDead = o.FCases.Count(o => o.IsF0 && o.IsDeath).ToString("#,##"),
                    TotalFromFx = o.FCases.Count(o => o.IsF0 && o.IsFx2F0).ToString("#,##"),
                    TotalFromF1 = o.FCases.Count(o => o.IsF0 && o.IsFx2F0 && o.LevelInitial == 1).ToString("#,##"),
                    TotalFromF2 = o.FCases.Count(o => o.IsF0 && o.IsFx2F0 && o.LevelInitial == 2).ToString("#,##"),
                    TotalFromF3 = o.FCases.Count(o => o.IsF0 && o.IsFx2F0 && o.LevelInitial == 3).ToString("#,##"),

                    TotalF1 = o.FCases.Count(o => o.LevelInitial == 1).ToString("#,##"),
                    TotalF2 = o.FCases.Count(o => o.LevelInitial == 2).ToString("#,##"),
                    TotalF3 = o.FCases.Count(o => o.LevelInitial == 3).ToString("#,##"),
                }).ToList();
            }
            return View(model);
        }
        #endregion
        #region Báo cáo số ca covid theo biểu đồ
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.FCaseReport, AppEnum.Covid })]
        [Route("bao-cao/bieu-do", Name = "ReportFCaseChart")]
        public async Task<IActionResult> FCaseChart()
        {
            var epidemicAreas = await _repository.GetRepository<EpidemicArea>().GetAllAsync();

            var abc = await _repository.GetRepository<FCase>().GetAllAsync();
            var model = new List<Report1>();

            if (epidemicAreas != null && epidemicAreas.Any())
            {
                model = epidemicAreas.Select(o => new Report1()
                {
                    OutbreakDate = o.OutbreakDate,
                    EpidemicAreaName = o.Name,
                    TotalF0 = o.FCases.Count(o => o.IsF0).ToString("#,##"),
                    IsCured = o.FCases.Count(o => o.IsF0 && o.IsCured).ToString("#,##"),
                    UnderTreatment = o.FCases.Count(o => o.IsF0 && !o.IsCured && !o.IsDeath).ToString("#,##"),
                    IsDead = o.FCases.Count(o => o.IsF0 && o.IsDeath).ToString("#,##"),
                    TotalFromFx = o.FCases.Count(o => o.IsF0 && o.IsFx2F0).ToString("#,##"),
                    TotalFromF1 = o.FCases.Count(o => o.IsF0 && o.IsFx2F0 && o.LevelInitial == 1).ToString("#,##"),
                    TotalFromF2 = o.FCases.Count(o => o.IsF0 && o.IsFx2F0 && o.LevelInitial == 2).ToString("#,##"),
                    TotalFromF3 = o.FCases.Count(o => o.IsF0 && o.IsFx2F0 && o.LevelInitial == 3).ToString("#,##"),

                    TotalF1 = o.FCases.Count(o => o.LevelInitial == 1).ToString("#,##"),
                    TotalF2 = o.FCases.Count(o => o.LevelInitial == 2).ToString("#,##"),
                    TotalF3 = o.FCases.Count(o => o.LevelInitial == 3).ToString("#,##"),
                }).ToList();
            }
            //var Date = _repository.GetRepository<FCase>().GetAll(o => o.IsF0).OrderBy(o => o.F0Date).Select(o => new
            //{
            //    Date = o.F0Date.Value.ToString("dd/MM")
            //}).Distinct();
            //var Data = _repository.GetRepository<FCase>().GetAll(o => o.IsF0).OrderBy(o => o.F0Date).Select(o => new
            //{
            //    Date = o.F0Date.Value.ToString("dd/MM")
            //}).Distinct();
            var data = _repository.GetRepository<FCase>().GetAll(o => o.IsF0).OrderBy(o => o.F0Date);
            foreach (var line in data.GroupBy(info => info.F0Date.Value.Date)
                        .Select(group => new
                        {
                            Metric = group.Key,
                            Count = group.Count()
                        })
                        //.OrderBy(x => x.Metric)
                        )
            {
                //Console.WriteLine("{0} {1}", line.Metric, line.Count);
            }

            var bar = data
            .GroupBy(n => n.F0Date.Value.Date)
            .Select(n => new BarChartModel()
            {
                Date = n.Key.ToString("dd/MM"),
                Data = n.Count()
            }
            ).ToList()
            //.OrderBy(n => n.MetricName)
            ;
            ViewData["groups"] = bar;

            return View(model);
        }
        [Route("report/chart-json-bar")]
        public async Task<IActionResult> ChartJsonBar(long id)
        {
            var data = _repository.GetRepository<FCase>().GetAll(o => o.IsF0).OrderBy(o => o.F0Date);
            var bar = data
               .GroupBy(n => n.F0Date.Value.Date)
               .Select(n => new BarChartModel()
               {
                   Date = n.Key.ToString("dd/MM"),
                   Data = n.Count()
               }
               ).ToList();


            var ea = await _repository.GetRepository<EpidemicArea>().ReadAsync(id);
            return Json(new
            {
                Date = bar.Select(o => o.Date),
                Data = bar.Select(o => o.Data)
            });
        }
        #endregion

        #region Báo cáo số ca covid theo biểu đồ 2
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.FCaseReport, AppEnum.Covid })]
        [Route("bao-cao/bieu-do2", Name = "ReportFCaseChart2")]
        public async Task<IActionResult> FCaseChart2()
        {
            var epidemicAreas = await _repository.GetRepository<EpidemicArea>().GetAllAsync();

            var abc = await _repository.GetRepository<FCase>().GetAllAsync();
            var model = new List<Report1>();

            if (epidemicAreas != null && epidemicAreas.Any())
            {
                model = epidemicAreas.Select(o => new Report1()
                {
                    OutbreakDate = o.OutbreakDate,
                    EpidemicAreaName = o.Name,
                    TotalF0 = o.FCases.Count(o => o.IsF0).ToString("#,##"),
                    IsCured = o.FCases.Count(o => o.IsF0 && o.IsCured).ToString("#,##"),
                    UnderTreatment = o.FCases.Count(o => o.IsF0 && !o.IsCured && !o.IsDeath).ToString("#,##"),
                    IsDead = o.FCases.Count(o => o.IsF0 && o.IsDeath).ToString("#,##"),
                    TotalFromFx = o.FCases.Count(o => o.IsF0 && o.IsFx2F0).ToString("#,##"),
                    TotalFromF1 = o.FCases.Count(o => o.IsF0 && o.IsFx2F0 && o.LevelInitial == 1).ToString("#,##"),
                    TotalFromF2 = o.FCases.Count(o => o.IsF0 && o.IsFx2F0 && o.LevelInitial == 2).ToString("#,##"),
                    TotalFromF3 = o.FCases.Count(o => o.IsF0 && o.IsFx2F0 && o.LevelInitial == 3).ToString("#,##"),

                    TotalF1 = o.FCases.Count(o => o.LevelInitial == 1).ToString("#,##"),
                    TotalF2 = o.FCases.Count(o => o.LevelInitial == 2).ToString("#,##"),
                    TotalF3 = o.FCases.Count(o => o.LevelInitial == 3).ToString("#,##"),
                }).ToList();
            }
            //var Date = _repository.GetRepository<FCase>().GetAll(o => o.IsF0).OrderBy(o => o.F0Date).Select(o => new
            //{
            //    Date = o.F0Date.Value.ToString("dd/MM")
            //}).Distinct();
            //var Data = _repository.GetRepository<FCase>().GetAll(o => o.IsF0).OrderBy(o => o.F0Date).Select(o => new
            //{
            //    Date = o.F0Date.Value.ToString("dd/MM")
            //}).Distinct();
            var data = _repository.GetRepository<FCase>().GetAll(o => o.IsF0).OrderBy(o => o.F0Date).ToList();
            foreach (var line in data.GroupBy(info => info.F0Date.Value.Date)
                        .Select(group => new
                        {
                            Metric = group.Key,
                            Count = group.Count()
                        })
                        //.OrderBy(x => x.Metric)
                        )
            {
                //Console.WriteLine("{0} {1}", line.Metric, line.Count);
            }

            var bar = data
            .GroupBy(n => n.F0Date.Value.Date)
            .Select(n => new BarChartModel()
            {
                Date = n.Key.ToString("dd/MM"),
                Data = n.Count()
            }
            ).ToList()
            //.OrderBy(n => n.MetricName)
            ;
            ViewData["groups"] = bar;

            return View(model);
        }
        [Route("report/chart-json-bar2")]
        public IActionResult ChartJsonBar2(long id)
        {
            var data = _repository.GetRepository<FCase>().GetAll(o => o.IsF0).OrderBy(o => o.F0Date).ToList();
            var bar = data
               .GroupBy(n => n.F0Date.Value.Date)
               .Select(n => new BarChartModel()
               {
                   Date = n.Key.ToString("dd/MM"),
                   //Tooltip = n.Key.ToString("dd/MM/yyyy"),
                   Data = n.Count()
               }
               ).ToList();



            return Json(new
            {
                Date = bar.Select(o => o.Date),
                Data = bar.Select(o => o.Data)
            });
        }
        [Route("report/chart-json-line")]
        public IActionResult ChartJsonLine()
        {
            var data = _repository.GetRepository<FCase>().GetAll(o => o.IsF0).OrderBy(o => o.F0Date).ToList();

            var dataByDate = data
                      .GroupBy(n => n.F0Date.Value.Date)
                      .Select(n => new
                      {
                          Date = n.Key,
                          ToTalByDate = n.Count()
                      });

            var count = 0;
            var cumulativeByDate = dataByDate.AsEnumerable().Select(x =>
            {
                count += x.ToTalByDate;
                var result = new
                {
                    Date = x.Date.ToString("dd/MM"),
                    ToTalByDate = x.ToTalByDate,
                    Cumulative = count
                };
                return result;
            }).ToList();
            return Json(new
            {
                Date = cumulativeByDate.Select(o => o.Date),
                ToTalByDate = cumulativeByDate.Select(o => o.ToTalByDate),
                Cumulative = cumulativeByDate.Select(o => o.Cumulative)
            });
        }
        [Route("report/chart-json-bar-hor")]
        public IActionResult ChartJsonBarHor()
        {
            //var ids = _repository.GetRepository<FCase>().GetAll(o => o.IsF0).Select(o => o.AddressCommuneId).Distinct().ToList();
            //var communes = _repository.GetRepository<Commune>().GetAll(o => ids.Contains(o.Id));

            var data = _repository.GetRepository<FCase>().GetAll(o => o.IsF0).OrderBy(o => o.F0Date).ToList();
            var bar = data
               .GroupBy(n => n.AddressCommuneId)
               .Select(n => new 
               {
                   Commune = GetAddressByCommune2(n.Key, true),
                   Data = n.Count()
               }
               ).OrderByDescending(o=>o.Data).ToList();

            return Json(new
            {
                Commune = bar.Select(o => o.Commune),
                Data = bar.Select(o => o.Data)
            });
        }
        #endregion


        #region FlotChartIndex
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.FCaseReport, AppEnum.Covid })]
        [Route("report", Name = "ReportIndex")]
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
            ViewData["EpidemicArea"] = new SelectList(epidemicAreaList, "Id", "Name", epidemicAreaList.Any() ? epidemicAreaList.First().Id : "");
            return View();
        }
        #endregion

        #region Báo cáo số ca lây nhiễm
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.FCaseReport, AppEnum.Covid })]
        [Route("report1", Name = "ReportIndex1")]
        public async Task<IActionResult> Index1()
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
            ViewData["EpidemicArea"] = new SelectList(epidemicAreaList, "Id", "Name", epidemicAreaList.Any() ? epidemicAreaList.First().Id : "");
            return View();
        }
        #endregion








        #region GetAddressByCommune
        private string GetAddressByCommune(long id)
        {
            string addr = "";
            if (id == 0)
                addr = "";
            try
            {
                var commune = _repository.GetRepository<Commune>().Read(id);
                if (commune != null)
                    addr = string.Format(@"{0}, {1}, {2}", commune.Name, commune.District.Name, commune.District.Province.Name);
            }
            catch { }
            return addr;
        }
        #endregion

        #region GetAddressByCommune2
        private string GetAddressByCommune2(long id, bool shortName = false)
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
