using GoTogether.API.Extensions;
using GoTogether.Application;
using GoTogether.Infrastructure;
using GoTogether.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAuthorization();

builder.Services.AddApiSecurity(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddInfrastructurePaths();

if (builder.Environment.IsDevelopment())
    builder.Services.AddDevTools();

var app = builder.Build();

app.UseApiSecurityConfiguration();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<GoTogetherDbContext>();
    db.Database.Migrate();
    DbInitializer.Seed(db);

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseImageLogging();
app.UseUploadsStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
