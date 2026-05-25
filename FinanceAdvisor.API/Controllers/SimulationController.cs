using FinanceAdvisor.Application.DTOs;
using FinanceAdvisor.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAdvisor.API.Controllers;

[ApiController]
[Route("api/investments")]
public class SimulationController(SimulationService service) : ControllerBase
{
    [HttpPost("simulate")]
    public IActionResult Simulate(SimulationRequest request) =>
        Ok(service.Simulate(request));
}
