using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Data;
using TicTacToe.Interfaces;
using TicTacToe.Repository;
using TicTacToe.Services;
using Hellang.Middleware.ProblemDetails;
using TicTacToe.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Добавляем Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (ctx, ex) => false;
    
    options.Map<ApiException.GameNotFoundException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<ApiException.PlayerNotFoundException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<ApiException.GameAlreadyCompletedException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<ApiException.ValidationException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<ApiException.InvalidCellException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<ApiException.WrongPlayerTurnException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
});

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IETagService, ETagService>();

var app = builder.Build();

app.UseProblemDetails();
// Включаем Swagger UI в Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
