using Common.Helpers;
using DataAccess;
using Entities;
using Entities.Enums;
using Entities.ViewModels;
using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Filters;
using Web.Helpers;

namespace Web.Controllers
{
    public class SharedController : BaseController
    {
        public SharedController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache) { }
        #region get-district-by-province
        [Route("shared/get-district-by-province/{id}")]
        public IActionResult GetDistrictByProvince(long id)
        {
            try
            {
                var districts = _repository.GetRepository<District>().GetAll(o => o.ProvinceId == id);
                var list = districts.OrderBy(o => o.Id).Select(o => new District
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList().ToSelectList();
                return Json(list);
            }
            catch
            {
                return Json(new List<District>().ToSelectList());
            }
        }
        #endregion
        #region get-commune-by-district
        [Route("shared/get-commune-by-district/{id}")]
        public IActionResult GetCommuneByDistrict(long id)
        {
            try
            {
                var communes = _repository.GetRepository<Commune>().GetAll(o => o.DistrictId == id);
                var list = communes.OrderBy(o => o.Id).Select(o => new Commune
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList().ToSelectList();
                return Json(list);
            }
            catch
            {
                return Json(new List<Commune>().ToSelectList());
            }
        }
        #endregion
        #region get-epidemic-area-by-commune
        [Route("shared/get-epidemic-area-by-commune/{id}")]
        public IActionResult GetEpidemicAreaByCommune(long id)
        {
            try
            {
                var epidemicAreas = _repository.GetRepository<EpidemicArea>().GetAll(o => o.CommuneId == id);
                var list = epidemicAreas.OrderBy(o => o.Id).Select(o => new EpidemicArea
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList().ToSelectList();
                return Json(list);
            }
            catch
            {
                return Json(new List<EpidemicArea>().ToSelectList());
            }
        }
        #endregion

        #region Tải lên tệp đính kèm
        [Route("shared/fcase/upload-files/{fcaseId}")]
        [HttpPost]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Create, ModuleEnum.FCaseDocument, AppEnum.Covid })]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, long fcaseId)
        {
            var fileUploadReturns = new List<FileUploadReturn>();
            try
            {
                var maxFileSize = 8;
                long size = files.Sum(f => f.Length);
                string att = "";
                try
                {
                    string fullFilePath = "/Uploads/fcase/" + fcaseId + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + fullFilePath);
                    try
                    {
                        if (!Directory.Exists(filePath))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(filePath);
                        }
                    }
                    catch { }
                    foreach (var file in files)
                    {
                        if (file != null && file.Length > 0)
                        {
                            var contentLength = (file.Length / 1024) / 1024;

                            if (contentLength < maxFileSize)
                            {
                                string type = Path.GetExtension(file.FileName).ToLower();
                                string docType = "fa fa-file";
                                if (type.Contains("xls"))
                                    docType = "fa fa-file-excel-o";
                                if (type.Contains("doc"))
                                    docType = "fa fa-file-word-o";
                                if (type.Contains("pdf"))
                                    docType = "fa fa-file-pdf-o";
                                if (type.Contains("jpg") || type.Contains("png") || type.Contains("bmp") || type.Contains("tif") || type.Contains("gif"))
                                    docType = "fa fa-file-image-o";
                                if (type.Contains("txt"))
                                    docType = "fa fa-file-text-o";
                                if (type.Contains("zip") || type.Contains("rar") || type.Contains("7z"))
                                    docType = "fa fa-file-archive-o";
                                if (type.Contains("ppt"))
                                    docType = "fa fa-file-powerpoint-o";

                                string fileName = CommonHelper.ToURL(Path.GetFileNameWithoutExtension(file.FileName), 0) + Path.GetExtension(file.FileName);
                                var path = Path.Combine(filePath, fileName);
                                if (System.IO.File.Exists(path))
                                {
                                    fileName = string.Format("{0}_{1}{2}", CommonHelper.ToURL(Path.GetFileNameWithoutExtension(file.FileName), 0), StringHelper.CreateRandomString(4), Path.GetExtension(file.FileName));
                                    path = Path.Combine(filePath, fileName);
                                }
                                using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }
                                att = fullFilePath + fileName;

                                FCaseDocument fd = new()
                                {
                                    CreateBy = AccountId,
                                    UpdateBy = AccountId,
                                    DocumentPath = att,
                                    DocumentType = docType,
                                    DocumentDate = DateTime.Now,
                                    FCaseId=fcaseId,
                                };
                                int x = _repository.GetRepository<FCaseDocument>().Create(fd, AccountId);
                                FileUploadReturn fileUploadReturn = new()
                                {
                                    name = fileName,
                                    size = file.Length,
                                    url = att,
                                    thumbnailUrl = att,
                                    deleteUrl = "/shared/fcase/delete-file?file=" + att + "&id=" + fd.Id,
                                    delete_url = "/shared/fcase/delete-file?file=" + att + "&id=" + fd.Id,
                                    deleteType = "DELETE",
                                    delete_type = "DELETE",
                                };
                                fileUploadReturns.Add(fileUploadReturn);
                            }
                        }
                    }
                }
                catch { }
                return Json(new { files = fileUploadReturns });
            }
            catch (Exception ex)
            {
                return Json(new { files = ex.Message });
            }
        }
        #endregion
        #region Xóa tệp đính kèm
        [Route("shared/fcase/delete-file")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.FCaseDocument, AppEnum.Covid })]
        public IActionResult DeleteFile(string file, long id)
        {
            try
            {
                try
                {
                    var fd = _repository.GetRepository<FCaseDocument>().Read(id);
                    int x = _repository.GetRepository<FCaseDocument>().Delete(fd, AccountId);
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
                catch { }
                return Json(new { files = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { files = ex.Message });
            }
        }
        #endregion
        #region Xóa tệp đính kèm 2
        [Route("shared/fcase/delete-file-2/{id}")]
        [TypeFilter(typeof(ValidationPermission), Arguments = new object[] { ActionEnum.Delete, ModuleEnum.FCaseDocument, AppEnum.Covid })]
        public IActionResult DeleteFile2(long id)
        {
            try
            {
                try
                {
                    var fd = _repository.GetRepository<FCaseDocument>().Read(id);
                    string file = fd.DocumentPath;
                    int x = _repository.GetRepository<FCaseDocument>().Delete(fd, AccountId);
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
                catch { }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion
        #region Lấy danh sách tệp đính kèm
        private string GetListDocument(List<FCaseDocument> docs)
        {
            string returnVal = "";
            if (docs != null && docs.Any())
            {
                returnVal = string.Concat(docs.Select(i => string.Format("<a href='{0}' target='_blank' title='{1}'><i class='{2}'></i></a><br />", i.DocumentPath, Path.GetFileName(i.DocumentPath), i.DocumentType)));
            }
            return returnVal;
        }
        #endregion

    }
}
