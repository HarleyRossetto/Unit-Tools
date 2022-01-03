using MQHandbookAPI.Models.Macquarie;
using MQHandbookLib.src.Helpers;
using MQHandbookLib.src.Macquarie.Handbook;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSingleton<IMacquarieHandbook, MacquarieHandbook>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

//Configure class mappings via profile.
builder.Services.AddAutoMapper(cfg => {
    cfg.AddProfile<MacquarieDtoMappingProfile>();
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
