using System.Net;
using GitHubApi.CatchedServices;
using Service.Extensions;
using Service.Services;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.Configure<ApiCredentials>(
//    builder.Configuration.GetSection("ApiCredentials"));
builder.Services.AddApiCredentials(options=> builder.Configuration.GetSection("ApiCredentials").Bind(options));

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IGitHubService, GitHubService>();
builder.Services.Decorate<IGitHubService, CachedGitHubServices>();
//var token = builder.Configuration["GitHubToken"];

var app = builder.Build();

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
