using Microsoft.EntityFrameworkCore;
using TodoApp.Model;
using Npgsql;
using Task = TodoApp.Model.Task;

namespace TodoApp.Database;

public class DbConn : DbContext
{
    public DbSet<Task> Tasks { get; set; }
    public readonly NpgsqlConnection Connection;

    public DbConn(IConfiguration configuration)
    {
        var connectionString =  configuration.GetConnectionString("DefaultConnection");
        Connection = new NpgsqlConnection(connectionString);
    }
}
