using Microsoft.EntityFrameworkCore;
using sustAInableEducation_backend.Models;
using sustAInableEducation_backend.Repository;
using sustAInableEducation_backend.Controllers;
using sustAInableEducation_backend.Hubs;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "sustAInableEducation API", Version = "v1" });
    options.AddSignalRSwaggerGen();
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDatabase")));
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddTransient<IAIService, AITestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapGroup("/account").MapIdentityApi<ApplicationUser>().WithTags("Account");

app.MapHub<EnvironmentHub>("/environmentHub/{id}");

app.Run();
