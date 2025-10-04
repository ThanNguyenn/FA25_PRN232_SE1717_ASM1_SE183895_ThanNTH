using EVServiceCenter.Repositories.ThanNTH.DBContext;
using EVServiceCenter.Repositories.ThanNTH.ModelExtensions;
using EVServiceCenter.Repositories.ThanNTH.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace EVServiceCenter.MVCWebApp.ThanNTH.Controllers
{

    [Authorize]
    public class CenterPartThanNthController : Controller
    {
        private string APIEndPoint = "https://localhost:7211/api/";
        public CenterPartThanNthController()
        {

        }

        //public async Task<IActionResult> Index(string? PartStatus, int? AvailableQuantity, string? PartName, int? CurrentPage)
        //{
        //    try
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            var tokenString = HttpContext.Request.Cookies["TokenString"];
        //            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
        //            using (var response = await httpClient.GetAsync(APIEndPoint + "CenterPartThanNths"))
        //            {
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    var content = await response.Content.ReadAsStringAsync();
        //                    var result = JsonConvert.DeserializeObject<List<CenterPartThanNth>>(content);
        //                    if (result != null)
        //                    {
        //                        return View(result);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    ModelState.AddModelError("", "Login failure");
        //    return View();
        //}

        public async Task<IActionResult> Index(string? PartStatus, int? AvailableQuantity, string? PartName, int? CurrentPage, int? PageSize)
        {
            var searchRequest = new CenterPartThanNthSearchRequest()
            {
                CurrentPage = CurrentPage ?? 1,
                PageSize = PageSize ?? 10,  
                PartStatus = PartStatus,
                AvailableQuantity = AvailableQuantity,
                PartName = PartName
            };

            ViewData["SearchRequest"] = searchRequest;

            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                using (var response = await httpClient.PostAsJsonAsync(APIEndPoint + "CenterPartThanNths/SearchWithPaging", searchRequest))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<PaginationResult<List<CenterPartThanNth>>>(content);

                        if (result != null)
                        {
                            var pagedList = new StaticPagedList<CenterPartThanNth>(
                                result.Items,
                                result.CurrentPage,
                                result.PageSize, 
                                result.TotaItems   
                            );

                            return View(pagedList);
                        }
                    }
                }
            }

            throw new Exception("Failed to load data from API.");
        }


        //public async Task<IActionResult> Create(CenterPartThanNth? model)
        //{
        //    try
        //    {
        //        if (HttpContext.Request.Method == "POST")
        //        {

        //            using (var httpClient = new HttpClient())
        //            {
        //                var token = HttpContext.Request.Cookies["TokenString"];
        //                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

        //                var response = await httpClient.PostAsJsonAsync(APIEndPoint + "CenterPartThanNths", model);
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    return RedirectToAction(nameof(Index));
        //                }

        //                ModelState.AddModelError("", "");
        //            }
        //        }


        //        await LoadDropdowns();
        //        return View(model ?? new CenterPartThanNth());
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        private async Task LoadDropdowns()
        {
            using (var httpClient = new HttpClient())
            {
                var token = HttpContext.Request.Cookies["TokenString"];
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);


                var responseParts = await httpClient.GetAsync(APIEndPoint + "PartThanNths");
                if (responseParts.IsSuccessStatusCode)
                {
                    var partContent = await responseParts.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<PartThanNth>>(partContent);



                    ViewData["PartId"] = new SelectList(result, "PartThanNthid", "PartName");
                }

                var responseCenters = await httpClient.GetAsync(APIEndPoint + "ServiceCenterHieuPTs");
                if (responseCenters.IsSuccessStatusCode)
                {
                    var centerContent = await responseCenters.Content.ReadAsStringAsync();
                    var centers = JsonConvert.DeserializeObject<List<ServiceCenterHieupt>>(centerContent);

                    ViewData["CenterId"] = new SelectList(centers ?? new List<ServiceCenterHieupt>(), "ServiceCenterHieuptId", "ServiceCenterName");
                }
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var token = HttpContext.Request.Cookies["TokenString"];
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    using (var response = await httpClient.GetAsync(APIEndPoint + "PartThanNths"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<List<PartThanNth>>(content);

                            if (result != null)
                            {
                                ViewData["PartId"] = new SelectList(result, "PartThanNthid", "PartName");
                                //ViewData["CenterId"] = new SelectList(result, "ServiceCenterHieuptId", "Address");
                            }
                        }
                    }
                    using (var response = await httpClient.GetAsync(APIEndPoint + "ServiceCenterHieuPTs"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<List<ServiceCenterHieupt>>(content);
                            if (result != null)
                            {
                                //ViewData["PartId"] = new SelectList(result, "CenterPartThanNthid", "PartName");
                                ViewData["CenterId"] = new SelectList(result, "ServiceCenterHieuptId", "ServiceCenterName");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(new CenterPartThanNth()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsDeleted = false
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CenterPartThanNth centerPartThanNth)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var httpClient = new HttpClient())
                    {
                        var token = HttpContext.Request.Cookies["TokenString"];
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                        var response = await httpClient.PostAsJsonAsync(APIEndPoint + "CenterPartThanNths", centerPartThanNth);
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<int>(content);

                            if (result > 0)
                            {
                                return RedirectToAction(nameof(Index));
                            }

                        }

                        using (var responseParts = await httpClient.GetAsync(APIEndPoint + "PartThanNths"))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var partContent = await responseParts.Content.ReadAsStringAsync();
                                var result = JsonConvert.DeserializeObject<List<PartThanNth>>(partContent);

                                
                                if (result !=null)
                                ViewData["PartId"] = new SelectList(result, "PartThanNthid", "PartName");
                            }
                        }

                        using (var responseCenters = await httpClient.GetAsync(APIEndPoint + "ServiceCenterHieuPTs"))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var centerContent = await responseCenters.Content.ReadAsStringAsync();
                                var centers = JsonConvert.DeserializeObject<List<ServiceCenterHieupt>>(centerContent);

                                if (centers != null)
                                 ViewData["CenterId"] = new SelectList(centers, "ServiceCenterHieuptId", "ServiceCenterName");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return View(centerPartThanNth);
        }


        public async Task<IActionResult> Edit(int? id, CenterPartThanNth? model)
        {
            try
            {
                if (HttpContext.Request.Method == "POST")
                {
                    using (var httpClient = new HttpClient())
                    {
                        var token = HttpContext.Request.Cookies["TokenString"];
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                        var response = await httpClient.PutAsJsonAsync(APIEndPoint + "CenterPartThanNths", model);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }

                        ModelState.AddModelError("", "API returned: " + response.StatusCode);
                    }
                }
                else
                {
                    using (var httpClient = new HttpClient())
                    {
                        var token = HttpContext.Request.Cookies["TokenString"];
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                        var response = await httpClient.GetAsync(APIEndPoint + "CenterPartThanNths/" + id);
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            model = JsonConvert.DeserializeObject<CenterPartThanNth>(content);
                        }
                    }
                }

                await LoadDropdowns();
                return View(model ?? new CenterPartThanNth());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            using (var httpClient = new HttpClient())
            {
                var token = HttpContext.Request.Cookies["TokenString"];
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var response = await httpClient.GetAsync($"{APIEndPoint}CenterPartThanNths/{id}");
                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var content = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<CenterPartThanNth>(content);

                if (model == null)
                    return NotFound();

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            using (var httpClient = new HttpClient())
            {
                var token = HttpContext.Request.Cookies["TokenString"];
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var response = await httpClient.GetAsync(APIEndPoint + "CenterPartThanNths/" + id);
                if (!response.IsSuccessStatusCode) return NotFound();

                var content = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<CenterPartThanNth>(content);

                return View(model);
            }
        }

        
        [HttpPost, ActionName("Delete")] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var token = HttpContext.Request.Cookies["TokenString"];
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var response = await httpClient.DeleteAsync(APIEndPoint + "CenterPartThanNths/" + id);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "API returned: " + response.StatusCode);
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

    
    }
}

