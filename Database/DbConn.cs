using Microsoft.EntityFrameworkCore;
using TodoApp.Model;
using Npgsql;

namespace TodoApp.Database;

public class DbConn : DbContext
{
    public DbSet<Tasks> Tasks { get; set; }
    public NpgsqlConnection Connection;

    public DbConn(IConfiguration configuration)
    {
        var connectionString =  configuration.GetConnectionString("DefaultConnection");
        Connection = new NpgsqlConnection(connectionString);
    }
}
