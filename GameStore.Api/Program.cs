using GameStore.Api.Data;
using GameStore.Api.Entities;
using GameStore.Api.AuthExtensions;
using GameStore.Api.GlobalExceptionHandler;
using GameStore.Api.Mapper;
using GameStore.Api.Services;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();//Controller added
//builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi("v1");

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddServices()
                .AddDataService(builder.Configuration);
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddJwtAuthorization();

var app = builder.Build();

//Seed initial Admin
app.Services.SeedAdmin();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "GameStore API";
    });
}



//Register Exception Middleware
app.UseGlobalExceptionHandler();

//Redirects Http client requests to Https
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//Registering Controllers
app.MapControllers(); 

app.Run();
