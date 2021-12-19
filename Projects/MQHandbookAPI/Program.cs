using Macquarie.Handbook;
using Macquarie.Handbook.Data;
using Macquarie.Handbook.Data.Course;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Data.Unit;
using Macquarie.Handbook.Data.Unit.Prerequisites;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Course;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Shared;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit.Prerequisites;
using CourseNoteDto = MQHandbookAPI.Models.Macquarie.Handbook.Data.Course.CourseNoteDto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddSingleton<IMacquarieHandbook, MacquarieHandbook>();

//Create all the Dto maps
builder.Services.AddAutoMapper(cfg => {
    cfg.AllowNullCollections = true;
    cfg.CreateMap<MacquarieUnit, UnitDto>();
    cfg.CreateMap<MacquarieUnitData, UnitDataDto>();

    cfg.CreateMap<LearningOutcome, LearningOutcomeDto>();
    cfg.CreateMap<EnrolmentRule, EnrolmentRuleDto>();
    cfg.CreateMap<Assessment, AssessmentDto>();
    
    cfg.CreateMap<UnitOffering, UnitOfferingDto>()
        .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Value))
        .ForMember(dest => dest.AcademicItem, opt => opt.MapFrom(src => src.AcademicItem.Value))
        .ForMember(dest => dest.AttendanceMode, opt => opt.MapFrom(src => src.AttendanceMode.Value))
        .ForMember(dest => dest.TeachingPeriod, opt => opt.MapFrom(src => src.TeachingPeriod.Value))
        .ForMember(dest => dest.StudyLevel, opt => opt.MapFrom(src => src.StudyLevel.Value));
    
    cfg.CreateMap<LearningActivity, LearningActivityDto>()
        .ForMember(dest => dest.Activity, opt => opt.MapFrom(src => src.Activity.Value));
    
    cfg.CreateMap<Requisite, RequisiteDto>()
        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.RequisiteType.Value));
    
    cfg.CreateMap<ContainerRequisiteTemporaryName, RequisiteContainerDto>()
        .ForMember(dest => dest.Connector, opt => opt.MapFrom(src => src.ParentConnector.Value));

    cfg.CreateMap<MacquarieCourse, CourseDto>();

    cfg.CreateMap<MacquarieCourseData, CourseDataDto>()
        .ForMember(dest => dest.AqfLevel, opt => opt.MapFrom(src => src.AqfLevel.Label))
        .ForMember(dest => dest.GovtSpecialCourseType, opt => opt.MapFrom(src => src.GovtSpecialCourseType.Value))
        .ForMember(dest => dest.PartnerFaculty, opt => opt.MapFrom(src => src.PartnerFaculty.Value))
        .ForMember(dest => dest.ExclusivelyAnExitAward, opt => opt.MapFrom(src => src.ExclusivelyAnExitAward.Label))
        .ForMember(dest => dest.VolumeOfLearning, opt => opt.MapFrom(src => src.VolumeOfLearning.Label))
        .ForMember(dest => dest.CourseDurationInYears, opt => opt.MapFrom(src => src.CourseDurationInYears.Label))
        .ForMember(dest => dest.CourseValue, opt => opt.MapFrom(src => src.CourseValue.Value));

    cfg.CreateMap<MacquarieCurriculumStructureData, CurriculumStructureDataDto>();
    cfg.CreateMap<UnitGroupingContainer, UnitGroupingContainerDto>()
        .ForMember(dest => dest.ParentRecord, opt => opt.MapFrom(src => src.ParentRecord.Value))
        .ForMember(dest => dest.ParentRecord, opt => opt.MapFrom(src => src.ParentRecord.Value));

    cfg.CreateMap<Offering, OfferingDto>()
        .ForMember(dest => dest.AdmissionCalendar, opt => opt.MapFrom(src => src.AdmissionCalendar.Value))
        .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Value))
        .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => src.Mode.Value))
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Value));

    cfg.CreateMap<AcademicItem, AcademicItemDto>()
        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Label));

    cfg.CreateMap<OrgUnitData, OrgUnitDataDto>();
    cfg.CreateMap<Fee, FeeDto>()
        .ForMember(dest => dest.FeeType, opt => opt.MapFrom(src => src.FeeType.Value));

    cfg.CreateMap<CourseNote, CourseNoteDto>()
        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Value));

    cfg.CreateMap<AdmissionRequirementPoint, AdmissionRequirementPointDto>()
       .ForMember(dest => dest.VolumeOfLearning, opt => opt.MapFrom(src => src.VolumeOfLearning.Label));

    cfg.CreateMap<LabelledValue, string>().ConvertUsing(src => src.Label);
    cfg.CreateMap<KeyValueIdType, string>().ConvertUsing(src => src.Value);

    cfg.CreateMap<MacquarieMaterialMetadata, MaterialMetadataDto>()
        .ForMember(dest => dest.AcademicOrganisation, opt => opt.MapFrom(src => src.AcademicOrganisation.Value))
        .ForMember(dest => dest.School, opt => opt.MapFrom(src => src.School.Value))
        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Value))
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Value));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
