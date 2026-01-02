using GameStore.Api.Data;
using GameStore.Api.GlobalExceptionHandler;
using GameStore.Api.Interfaces;
using GameStore.Api.Mapper;
using GameStore.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();//Controller added
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});


builder.Services.AddScoped<IGameServices, GamesServices>();
builder.Services.AddScoped<IGenresServices, GenreServices>();


var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseSqlite(connString));

var app = builder.Build();

//Register Exception Middleware
app.UseGlobalExceptionHandler();

//Redirects Http client requests to Https
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); 

app.Run();
