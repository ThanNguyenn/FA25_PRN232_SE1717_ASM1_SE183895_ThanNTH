using EVServiceCenter.Services.ThanNTH;
using Microsoft.AspNetCore.Mvc;

namespace EVServiceCenter.WebAPI.ThanNTH.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceCenterHieuPTsController : ControllerBase
{
    private readonly ServiceCenterHieuPTService _service;
    public ServiceCenterHieuPTsController(ServiceCenterHieuPTService service)
    {
        _service = service;
    }
    // GET: api/<ServiceCenterHieuPTsController>
    /// <summary>
    /// Get all Service Center HieuPTs
    /// </summary>
    /// <returns>List of Service Center HieuPTs</returns>

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAll();

        return Ok(response);
    }
}
