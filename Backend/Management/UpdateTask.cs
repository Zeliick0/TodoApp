using Npgsql;
using TodoApp.Database;
using TodoApp.Model;

namespace TodoApp.Management;

public class UpdateTask(DbConn dbConn)
{
    private const string UpdateQuery = "UPDATE tasks SET title = @title, description = @description, status = @status, priority = @priority WHERE id = @id";
    private const string CheckQuery = "SELECT status FROM TASKS where id = @id";
    public async Task<bool> UpdateTaskAsync(Tasks task)
    {
        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(UpdateQuery, dbConn.Connection);
        var check = new NpgsqlCommand(CheckQuery, dbConn.Connection);

        var reader = await check.ExecuteReaderAsync();
        await reader.ReadAsync();
        var status = reader.GetInt32(reader.GetOrdinal("status"));

        if (status == 2)
        {
            await dbConn.Connection.CloseAsync();
            return false;
        }
       
        command.Parameters.AddWithValue("@id", task.Id);
        command.Parameters.AddWithValue("@title", task.Title);
        command.Parameters.AddWithValue("@description", task.Description);
        command.Parameters.AddWithValue("@status", (int)task.Status);
        command.Parameters.AddWithValue("@priority", (int)task.Priority);
        await command.ExecuteNonQueryAsync();
        
        await dbConn.Connection.CloseAsync();
        return true;
    }
}