using Npgsql;
using TodoApp.Database;
using TodoApp.Model;

namespace TodoApp.Management;

public class CreateTask(DbConn dbConn)
{
    private const string AddQuery = "INSERT INTO tasks (title, description, status, priority) VALUES (@title, @description,  @status, @priority) RETURNING id;";

    public async Task<int> CreateTaskAsync(Tasks task)
    {
        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(AddQuery, dbConn.Connection);
        
        command.Parameters.AddWithValue("@title", task.Title);
        command.Parameters.AddWithValue("@description", task.Description);
        command.Parameters.AddWithValue("@status", (int)task.Status);
        command.Parameters.AddWithValue("@priority", (int)task.Priority);
        var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var id = reader.GetInt32(reader.GetOrdinal("id"));
        
        await  dbConn.Connection.CloseAsync();
        return id;
    }
}