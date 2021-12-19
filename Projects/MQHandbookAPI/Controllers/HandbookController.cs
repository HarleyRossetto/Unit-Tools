namespace MQHandbookAPI.Controllers;

using System.Threading.Tasks;
using AutoMapper;
using Macquarie.Handbook;
using Macquarie.Handbook.Data;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Helpers.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Course;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit;

[ApiController]
[Route("[controller]")]
public class HandbookController : ControllerBase
{
    private readonly ILogger<HandbookController> _logger;
    private readonly IMacquarieHandbook _handbook;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public HandbookController(ILogger<HandbookController> logger, IMacquarieHandbook handbook, IMapper mapper, IConfiguration configuration) {
        _logger = logger;
        _handbook = handbook;
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpGet("[action]/{unitCode}")]
    public async Task<UnitDto> GetUnit(string unitCode = "elec3042") {
        _logger.LogInformation("Attempting to retrieve data for {unitCode}", unitCode);
        return _mapper.Map<UnitDto>(await _handbook.GetUnit(unitCode));
    }

    [HttpGet("[action]/{courseCode}")]
    public async Task<CourseDto> GetCourse(string courseCode = "C000006") {
        _logger.LogInformation("Attempting to retrieve data for {unitCode}", courseCode);
        return _mapper.Map<CourseDto>(await _handbook.GetCourse(courseCode));
    }

    [HttpGet("[action]/{courseCode}")]
    public async Task<CurriculumStructureDataDto> GetCourseStructure(string courseCode = "C000006") {
        _logger.LogInformation("Attempting to retrieve data for {unitCode}", courseCode);
        return _mapper.Map<CurriculumStructureDataDto>((await _handbook.GetCourse(courseCode)).CurriculumData);
    }

    [HttpGet("[action]")]
    public async ValueTask<int> GetUnitCount(CancellationToken cancellationToken, int? implementationYear = null) {
        _logger.LogInformation("Attemping to retrieve all units to determine count");
        return (await _handbook.GetAllUnits(implementationYear: implementationYear, cancellationToken: cancellationToken)).Count;
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<string>> GetAllUnitCodes(int? implementationYear = null) {
        var allUnits = await _handbook.GetAllUnits(implementationYear ?? int.Parse(_configuration["DefaultYear"]));

        if (allUnits is null)
            return new string[] { "Unable to access all unit data." };

        return from unit in allUnits.Collection
               select unit.Code;
    }

    [HttpGet("[action]")]
    public async Task<Dictionary<string, Dictionary<string, int>>> GatherUnitStatistics() {
        //Get all units for default year.
        var units = await _handbook.GetAllUnits(int.Parse(_configuration["DefaultYear"]));

        //If results is null or count is 0, return 1 and finish.
        if (units is null || units.Count < 1)
            return new();

        _logger.LogInformation("Units loaded.");

        //Key: string (property name)
        //Value: Dictionary of string (value), and int (occurance count).
        Dictionary<string, Dictionary<string, int>> valueMap = new();

        foreach (var unit in units.Collection) {
            foreach (var outcome in unit.Data.LearningOutcomes) {

                var properties = outcome.GetType().GetProperties();
                foreach (var property in properties) {

                    //If the value map does not contain our property, create new map and add to map.
                    if (!valueMap.TryGetValue(property.Name, out Dictionary<string, int>? map)) {
                        map = new();
                        valueMap.Add(property.Name, map);
                    }

                    var propValue = property.GetValue(outcome);

                    //If value type dont need to inspect deeper
                    if (property.DeclaringType!.IsValueType) {
                        _logger.LogInformation("{type} is value type. Not logging.", property.Name);
                    } else {
                        var key = propValue?.ToString() ?? "null";
                        if (map.ContainsKey(key)) {
                            map[key]++;
                        } else {
                            map.Add(key, 1);
                        }
                    }
                }
            }
        }

        return valueMap;
    }
}
