using AnnouncemenetsAppAPI.Middlewares;
using AnnouncemenetsAppAPI.StartupExtensions;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(services);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseSecurityStampValidatorMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();
