using EVServiceCenter.Repositories.ThanNTH.ModelExtensions;
using EVServiceCenter.Repositories.ThanNTH.Models;
using EVServiceCenter.Services.ThanNTH;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EVServiceCenter.WebAPI.ThanNTH.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CenterPartThanNthsController : ControllerBase
{
    private readonly ICenterPartThanNthService _service;

    public CenterPartThanNthsController(ICenterPartThanNthService service)
    {
        _service = service;
    }
    // GET: api/<CenterPartThanNthsController>
    [EnableQuery]
    [Authorize(Roles = "1,2")]
    [HttpGet]
    public async Task<IEnumerable<CenterPartThanNth>> Get()
    {
        return await _service.GetAllAsync();
    }

    // GET api/<CenterPartThanNthsController>/5
    [Authorize(Roles = "1,2")]
    [HttpGet("{id}")]
    public async Task<CenterPartThanNth> Get(int id)
    {
        return await _service.GetByIdAsync(id);
    }

    // POST api/<CenterPartThanNthsController>
    [Authorize(Roles = "1,2")]
    [HttpPost]
    public async Task<int> Post(CenterPartThanNth centerPartThanNth)
    {
        return await _service.CreateAsync(centerPartThanNth);
    }

    // PUT api/<CenterPartThanNthsController>/5
    [Authorize(Roles = "1,2")]
    [HttpPut]
    public async Task<int> Put(CenterPartThanNth centerPartThanNth)
    {
        return await _service.UpdateAsync(centerPartThanNth);
    }

    // DELETE api/<CenterPartThanNthsController>/5
    [Authorize(Roles = "1")]
    [HttpDelete("{id}")]
    public Task<bool> Delete(int id)
    {
        return _service.DeleteAsync(id);
    }

    // GET: api/<CenterPartThanNthsController>
    [Authorize(Roles = "1,2")]
    [HttpGet("Search")]
    public async Task<IEnumerable<CenterPartThanNth>> Search(string? partStatus, int? availableQuantity, string? partName)
    {
        return await _service.SearchAsync(partStatus, availableQuantity, partName);
    }

    // GET: api/<CenterPartThanNthsController>
    [Authorize(Roles = "1,2")]
    [HttpPost("SearchWithPaging")]
    public async Task<PaginationResult<List<CenterPartThanNth>>> SearchWithPaging(CenterPartThanNthSearchRequest centerPartThanNthSearchRequest)
    {
        return await _service.SearchWithPagingAsync(centerPartThanNthSearchRequest);
    }
}
