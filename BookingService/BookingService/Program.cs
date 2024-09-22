using BookingService;


var builder = WebApplication.CreateBuilder(args);

await builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddlewares();

app.Run();
