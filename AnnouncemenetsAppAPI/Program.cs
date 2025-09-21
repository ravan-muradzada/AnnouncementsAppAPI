using AnnouncemenetsAppAPI.Middlewares;
using AnnouncemenetsAppAPI.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

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
