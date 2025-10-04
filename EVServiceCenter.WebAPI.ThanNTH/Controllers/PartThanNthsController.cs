using EVServiceCenter.Repositories.ThanNTH.Models;
using EVServiceCenter.Services.ThanNTH;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVServiceCenter.WebAPI.ThanNTH.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PartThanNthsController : Controller
{
    private readonly PartThanNthService _partThanNthService;

    public PartThanNthsController(PartThanNthService partThanNthService)
    {
        _partThanNthService = partThanNthService;
    }

    [Authorize]
    [HttpGet]
    public async Task<List<PartThanNth>> Get()
    {
        return await _partThanNthService.GetAllPartsAsync();
    }


}
