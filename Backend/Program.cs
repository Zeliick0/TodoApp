using TodoApp.CORS;
using TodoApp.Database;

var builder = WebApplication.CreateBuilder(args);

builder.AllowCorsService();
builder.Services.AddControllers();
builder.Services.AddDbContext<DbConn>();

var app = builder.Build();

app.UseCorsPolicy();
app.MapControllers();

app.Run();