using Application;
using Application.Middleware;
using Application.Models.Requests;
using Application.Profiles;
using Application.Validators;
using Asp.Versioning;
using Domain.Models.Requests;
using Domain.Ports.Primary;
using Domain.Ports.Secondary;
using Domain.Services;
using FluentValidation;
using Infrastructure;
using Infrastructure.Adapters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
    .AddMvc()
    .AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddAutoMapper(typeof(DayOffProfile));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DayOffManagerDbConnection")));

builder.Services.AddScoped<IDayOffRepository, DayOffRepository>();
builder.Services.AddTransient<IDayOffService, DayOffService>();

builder.Services.AddScoped<IValidator<DayOffCreateDto>, DayOffDtoValidator>();
builder.Services.AddScoped<IValidator<StatusReasonRequestDto>, StatusReasonRequestDtoValidator>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error while creating the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
