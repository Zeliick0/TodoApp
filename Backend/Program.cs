using TodoApp.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<DbConn>();

var app = builder.Build();

app.MapControllers();

app.Run();