using WebApi.Helpers;
using WebApi.Models.Jobs;

namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

[ApiController]
[Route("tibber-developer-test")]
public class JobController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet("hallo-welt")]
    public Task<IActionResult> HalloWelt()
    {
        return Task.FromResult<IActionResult>(Ok("Hallo Welt"));
    }

    [HttpPost("enter-path")]
    public async Task<IActionResult> EnterPath(EnterPathRequest enterPath)
    {
        var job = PathUtil.CreateJobFromEnterPathRequest(enterPath);
        job = await _jobService.Create(job);
        return Ok(job);
    }
}