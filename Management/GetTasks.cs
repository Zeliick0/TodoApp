using Npgsql;
using TodoApp.Database;
using TodoApp.Model;

namespace TodoApp.Management;

public class GetTasks(DbConn dbConn)
{
    private const string ListQuery = "SELECT * FROM tasks";
    
    public async Task<List<Tasks>> GetTasksAsync()
    {
     var tasks = new List<Tasks>();
     
     await dbConn.Connection.OpenAsync();
     var command = new NpgsqlCommand(ListQuery, dbConn.Connection);
     var reader = await command.ExecuteReaderAsync();

     while (await reader.ReadAsync())
     {
         var task = new Tasks
         {
             Id = reader.GetInt32(reader.GetOrdinal("id")),
             Title = reader.GetString(reader.GetOrdinal("title")),
             Description = reader.GetString(reader.GetOrdinal("description")),
             Status = (Status)reader.GetInt32(reader.GetOrdinal("status")),
             Priority = (Priority)reader.GetInt32(reader.GetOrdinal("priority")),
             CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
         };
         tasks.Add(task);
     }
     
     await dbConn.Connection.CloseAsync();
     return tasks;
    }
}