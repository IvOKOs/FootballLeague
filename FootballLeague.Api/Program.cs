using FootballLeague.Api.Database;
using FootballLeague.Api.Middleware;
using FootballLeague.Api.Services.Matches;
using FootballLeague.Api.Services.TeamRankings;
using FootballLeague.Api.Services.TeamRankings.PointsCalculation;
using FootballLeague.Api.Services.Teams;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<FootballDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ITeamRankingService, TeamRankingService>();
builder.Services.AddScoped<IFootballDbContext>(sp =>
    sp.GetRequiredService<FootballDbContext>());
builder.Services.AddScoped<IPointsStrategy, StandardPointsStrategy>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigrationsAsync();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapControllers();

await app.RunAsync();
