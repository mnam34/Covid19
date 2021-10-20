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
    public class ChartController : BaseController
    {
        #region General
        public ChartController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache) { }
        #endregion
        #region ChartIndex
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Read, ModuleEnum.FCase, AppEnum.Covid })]
        [Route("chart", Name = "ChartIndex")]
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
        #region OrgChart; load ajax: https://dabeng.github.io/OrgChart/multiple-layers.html
        [Route("chart-json/{id}")]
        public async Task<IActionResult> ChartJson(long id)
        {
            var children = await GetFCase(id);
            var ea = await _repository.GetRepository<EpidemicArea>().ReadAsync(id);
            return Json(new
            {
                id = "0",
                name = ea.Name,
                title = ea.Commune.Name, //ea.Name + ", " + GetAddressByCommune(ea.CommuneId),
                children = children,
                //img = "",
            });
        }
        private async Task<List<OrgChart>> GetFCase(long id)
        {
            var data = new List<OrgChart>();
            //Chỉ lấy F0 gốc, không lấy F0 từ Fx
            var genealogies = await _repository.GetRepository<FCase>().GetAllAsync(o => o.IsF0 && !o.IsFx2F0 && o.EpidemicAreaId == id);
            if (genealogies != null && genealogies.Any())
            {
                data.AddRange(genealogies.Select(o => new OrgChart
                {
                    id = o.Id.ToString(),
                    name = "F0-" + o.Name,
                    title = o.Name,
                    children = GetFCase2(o.FCases.ToList()),
                    className = "f0-level",
                    //collapsed = true,//Dùng cái này nếu muốn ẩn node nào đó
                }));
            }
            return data;
        }
        private List<OrgChart> GetFCase2(List<FCase> genealogies)
        {
            var data = new List<OrgChart>();
            if (genealogies != null && genealogies.Any())
            {
                data.AddRange(genealogies.Select(o => new OrgChart
                {
                    id = o.Id.ToString(),
                    name = o.IsF0 ? ("F0-" + o.Name) : ("F" + o.Level + "-" + o.Name),
                    title = o.Name,
                    children = GetFCase2(o.FCases.ToList()),
                    className = o.IsF0 ? "f0-level" : (o.Level == 1 ? "f1-level" : o.Level == 2 ? "f2-level" : "f3-level"),
                    //collapsed = true,//Dùng cái này nếu muốn ẩn node nào đó
                }));
            }
            return data;
        }
        #endregion
        #region Jit Chart // chưa test được
        [Route("chart-json-2")]
        public async Task<IActionResult> ChartJson2()
        {
            var children = await JitGetFCase();
            return Json(new
            {
                id = "0",
                name = "Cây gia phả",
                children = children,
            });
        }

        private async Task<List<OrgChart>> JitGetFCase()
        {
            var data = new List<OrgChart>();
            //Chỉ lấy F0 gốc, không lấy F0 từ Fx
            var genealogies = await _repository.GetRepository<FCase>().GetAllAsync(o => o.IsF0 && !o.IsFx2F0);
            if (genealogies != null && genealogies.Any())
            {
                data.AddRange(genealogies.Select(o => new OrgChart
                {
                    id = o.Id.ToString(),
                    name = "F0-" + o.Name,
                    children = JitGetFCase2(o.FCases.ToList()),
                }));
            }
            return data;
        }
        private List<OrgChart> JitGetFCase2(List<FCase> genealogies)
        {
            var data = new List<OrgChart>();
            if (genealogies != null && genealogies.Any())
            {
                data.AddRange(genealogies.Select(o => new OrgChart
                {
                    id = o.Id.ToString(),
                    name = o.IsF0 ? ("F0-" + o.Name) : ("F" + o.Level + "-" + o.Name),
                    children = JitGetFCase2(o.FCases.ToList()),
                }));
            }
            return data;
        }
        #endregion
        #region Cây JSON        
        [Route("chart-json3/{eaId}")]
        public async Task<IActionResult> ChartJson3(long eaId, string parent)
        {
            var ea = await _repository.GetRepository<EpidemicArea>().ReadAsync(eaId);
            var data = await _repository.GetRepository<FCase>().GetAllAsync(o => o.EpidemicAreaId == eaId);
            var jsTree = new List<JsTreeModel>
            {
                new JsTreeModel { id = "-1", parent="#", text = ea.Name, children = (data!= null && data.Any()) ? true : false, icon="fa fa-hospital-o", state=new JsTreeStateModel{opened=true,selected=true } }
            };
            if (parent == "parent")
            {
                return Json(jsTree);
            }
            if (data != null && data.Any())
            {
                var result = data;
                if (parent == "-1")
                {
                    result = data.Where(o => o.IsF0 && !o.IsFx2F0).ToList();
                }
                if (parent != "-1" && parent != "parent")
                {
                    var resultString = Regex.Match(parent, @"\d+").Value;
                    long id = long.Parse(resultString);
                    result = data.Where(o => o.FCaseId == id).ToList();
                }

                return Json(result.Select(o => new
                {
                    id = o.Id,
                    parent = (!string.IsNullOrEmpty(parent) && parent != "parent" && parent != "-1") ? parent : "-1",
                    text = o.Name,
                    children = o.FCases.Any() ? true : false,
                    icon = "fa fa-stethoscope",
                    //li_attr= "{ 'class' : 'tree-f0' }"
                    li_attr = new { style = o.IsF0 ? "color: #ed6b75;" : (o.Level == 1 ? "color: #F1C40F;" : o.Level == 2 ? "color: #337ab7;" : "color: #36c6d3;") },
                    state = new JsTreeStateModel { opened = true, selected = false }
                }));
            }
            else
            {
            }
            return Json(jsTree);
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
    }

}
