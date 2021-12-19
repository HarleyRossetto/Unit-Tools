namespace MQHandbookAPI.Controllers;

using System.Threading.Tasks;
using Macquarie.Handbook;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CourseLoopController : ControllerBase
{
    private readonly ILogger<HandbookController> _logger;
    private readonly IMacquarieHandbook _handbook;

    public CourseLoopController(ILogger<HandbookController> logger, IMacquarieHandbook handbook) {
        _logger = logger;
        _handbook = handbook;
    }

    [HttpGet(Name = "GetUnitCMSJson/{unitCode}")]
    public async Task<string> GetUnit(string unitCode) {
        _logger.LogInformation("Attempting to retrieve data for {unitCode}", unitCode);
        return await _handbook.GetUnitRawJson(unitCode);
    }

}
