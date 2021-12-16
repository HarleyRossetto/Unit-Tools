namespace MQHandbookAPI.Controllers;

using System.Threading.Tasks;
using AutoMapper;
using Macquarie.Handbook;
using Macquarie.Handbook.Data;
using Microsoft.AspNetCore.Mvc;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit;

[ApiController]
[Route("[controller]")]
public class HandbookController : ControllerBase
{
    private readonly ILogger<HandbookController> _logger;
    private readonly IMacquarieHandbook _handbook;
    private readonly IMapper _mapper;

    public HandbookController(ILogger<HandbookController> logger, IMacquarieHandbook handbook, IMapper mapper) {
        _logger = logger;
        _handbook = handbook;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetUnit/{unitCode}")]
    public async Task<UnitDTO> GetUnit(string unitCode = "elec3042") {
        _logger.LogInformation("Attempting to retrieve data for {unitCode}", unitCode);
        return _mapper.Map<UnitDTO>(await _handbook.GetUnit(unitCode));
    }
}
