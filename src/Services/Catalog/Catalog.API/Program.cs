var builder = WebApplication.CreateBuilder(args);

//Add services in the container

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddMarten(config =>
{
    config.Connection(builder.Configuration.GetConnectionString("Database"));   
}).UseLightweightSessions();

var app = builder.Build();

//Configure HTTP pipelines

app.MapCarter();

app.Run();
