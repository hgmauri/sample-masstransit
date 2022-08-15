using Sample.Masstransit.WebApi.Core;
using Sample.Masstransit.WebApi.Core.Extensions;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddSerilog("API MassTransit");
    Log.Information("Starting API");

    var appSettings = new AppSettings();
    builder.Configuration.Bind(appSettings);

    builder.Services.AddRouting(options => options.LowercaseUrls = true);

    builder.Services.AddControllers();
    builder.Services.AddOpenTelemetry(appSettings);

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", null);
    });

    builder.Services.AddMassTransitExtension(builder.Configuration);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample.Masstransit.WebApi v1"));
    }

    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}