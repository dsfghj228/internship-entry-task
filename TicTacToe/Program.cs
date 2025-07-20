using Microsoft.EntityFrameworkCore;
using TicTacToe.Data;
using TicTacToe.Interfaces;
using TicTacToe.Repository;
using TicTacToe.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Добавляем Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ETagService>();

var app = builder.Build();

// Включаем Swagger UI в Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
