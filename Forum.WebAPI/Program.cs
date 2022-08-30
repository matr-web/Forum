using Forum.WebAPI;
using Forum.WebAPI.Repositories;
using Forum.WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IAnswersRepository, AnswersRepository>();
builder.Services.AddScoped<IQuestionsRepository, QuestionsRepository>();

builder.Services.AddScoped<IAnswersService, AnswersService>();
builder.Services.AddScoped<IQuestionsService, QuestionsService>();

var app = builder.Build();

//Seed Data.
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<DatabaseContext>();
SeedData.GenerateUsers(dbContext);
SeedData.GenerateQuestions(dbContext);
SeedData.GenerateAnswers(dbContext);
SeedData.GenerateRatings(dbContext);

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
