using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Sample.Masstransit.WebApi.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Sample.Masstransit.WebApi", Version = "v1" });
});

builder.Services.AddMassTransit(bus =>
{
    bus.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
        cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(BusMessages.PublishClientInserted, false));
        cfg.UseMessageRetry(retry =>
        {
            retry.Interval(3, TimeSpan.FromSeconds(5));
        });
    });
});
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample.Masstransit.WebApi v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();