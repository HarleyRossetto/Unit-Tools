using Macquarie.Handbook;
using Macquarie.Handbook.Data;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Data.Unit;
using Macquarie.Handbook.Data.Unit.Prerequisites;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Shared;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit.Prerequisites;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddSingleton<IMacquarieHandbook, MacquarieHandbook>();
//builder.Services.AddSingleton<Unit_Info.Helpers.ILogger, HandbookLogger>();

builder.Services.AddAutoMapper(cfg => {
    cfg.AllowNullCollections = true;
    cfg.CreateMap<MacquarieUnit, UnitDTO>();
    cfg.CreateMap<MacquarieUnitData, UnitDataDTO>();

    cfg.CreateMap<LearningOutcome, LearningOutcomeDTO>();
    cfg.CreateMap<EnrolmentRule, EnrolmentRuleDTO>();
    cfg.CreateMap<Assessment, AssessmentDTO>();
    cfg.CreateMap<UnitOffering, UnitOfferingDTO>()
        .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Label))
        .ForMember(dest => dest.AcademicItem, opt => opt.MapFrom(src => src.AcademicItem.Value))
        .ForMember(dest => dest.AttendanceMode, opt => opt.MapFrom(src => src.AttendanceMode.Value))
        .ForMember(dest => dest.TeachingPeriod, opt => opt.MapFrom(src => src.TeachingPeriod.Value))
        .ForMember(dest => dest.StudyLevel, opt => opt.MapFrom(src => src.StudyLevel.Value));
    cfg.CreateMap<LearningActivity, LearningActivityDTO>()
        .ForMember(dest => dest.Activity, opt => opt.MapFrom(src => src.Activity.Value));

    cfg.CreateMap<LabelledValue, string>().ConvertUsing(src => src.Label);
    cfg.CreateMap<KeyValueIdType, string>().ConvertUsing(src => src.Value);

    cfg.CreateMap<MacquarieMaterialMetadata, MaterialMetadataDTO>()
        .ForMember(dest => dest.AcademicOrganisation, opt => opt.MapFrom(src => src.AcademicOrganisation.Value))
        .ForMember(dest => dest.School, opt => opt.MapFrom(src => src.School.Value))
        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Value))
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Value));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
