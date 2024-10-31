var builder = WebApplication.CreateBuilder(args);

//Add services in the container

var app = builder.Build();

//Configure HTTP pipelines

app.MapGet("/", () => "Hello World!");

app.Run();
