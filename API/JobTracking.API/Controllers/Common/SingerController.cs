using JobTracking.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace JobTracking.API.Controllers.Common;
[ApiController]
[Route("api/[controller]/[action]")]
public class SingerController : Controller
{
    private readonly ISingerService _singerService;
    public SingerController(ISingerService service)
    {
        _singerService = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        // Implementation to get a singer by ID
        return Ok(await _singerService.GetSinger(id));
    }
}