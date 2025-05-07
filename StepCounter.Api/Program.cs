using Scalar.AspNetCore;
using StepCounter.Api.Repositories;
using StepCounter.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<ITeamRepository, InMemoryTeamRepository>();
builder.Services.AddSingleton<TeamService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapOpenApi();

var serverUrl = builder.Configuration["App:ServerUrl"] ?? "http://localhost:7777";

app.MapScalarApiReference(options => {
    options
        .WithTitle("Team Step Counter Api")
        .WithTheme(ScalarTheme.Purple)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    options.Servers?.Clear();
    options.AddServer(serverUrl);
});

app.Run();
