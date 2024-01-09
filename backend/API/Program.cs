using System.Text.Json.Serialization;
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ISocialEventsService, SocialEventsService>();
builder.Services.AddScoped<ISocialEventsRepository, SocialEventsRepository>();
builder.Services.AddScoped<ISocialEventCompaniesService, SocialEventCompaniesService>();
builder.Services.AddScoped<ISocialEventCompaniesRepository, SocialEventCompaniesRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ISocialEventPersonsService, SocialEventPersonsService>();
builder.Services.AddScoped<ISocialEventPersonsRepository, SocialEventPersonsRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddDbContext<NullamDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure CORS for development environment to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize db
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<NullamDbContext>();
    
    // Apply migrations
    dbContext.Database.Migrate();
    
    // Seed db
    NullamDbContext.Initialize(dbContext);
}

// Swagger middleware conf
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();

app.Run();