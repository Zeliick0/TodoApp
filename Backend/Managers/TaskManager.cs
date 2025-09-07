using Microsoft.Data.SqlClient;
using TodoApp.Database;
using Npgsql;
using TodoApp.Model;
using TodoApp.Constants;
using Task = TodoApp.Model.Task;

namespace TodoApp.Managers;

public class TaskManager(DbConn dbConn)
{
    public async Task<int> CreateTaskAsync(Task task, int userId)
    {
        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(QueryConstants.Tasks.AddQuery, dbConn.Connection);
        
        command.Parameters.AddWithValue("@title", task.Title);
        command.Parameters.AddWithValue("@description", task.Description);
        command.Parameters.AddWithValue("@status", 0);
        command.Parameters.AddWithValue("@priority", (int)task.Priority);
        command.Parameters.AddWithValue("@user_id", userId);
        var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var id = reader.GetInt32(reader.GetOrdinal("id"));
        
        await  dbConn.Connection.CloseAsync();
        return id;
    }
    
    public async Task<bool> DeleteTaskAsync(int id, int userId)
    {
        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(QueryConstants.Tasks.DeleteQuery, dbConn.Connection);
        
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@user_id", userId);
        
        await  command.ExecuteNonQueryAsync();
        await dbConn.Connection.CloseAsync();
        return true;
    }

    public async Task<List<Task>> GetTasksByUserAsync(int userId)
    {
        var tasks = new List<Task>();

        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(QueryConstants.Tasks.GetQuery, dbConn.Connection);
        command.Parameters.AddWithValue("@user_id", userId);
        
        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var task = new Task
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Title = reader.GetString(reader.GetOrdinal("title")),
                Description = reader.GetString(reader.GetOrdinal("description")),
                Status = (Status)reader.GetInt32(reader.GetOrdinal("status")),
                Priority = (Priority)reader.GetInt32(reader.GetOrdinal("priority")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
            };
            tasks.Add(task);
        }

        await dbConn.Connection.CloseAsync();
        return tasks;
    }

    public async Task<bool> UpdateTaskAsync(Task task, int userId)
    {
        await dbConn.Connection.OpenAsync();

        try
        {
            var checkStatusCmd = new NpgsqlCommand(QueryConstants.Tasks.StatusQuery, dbConn.Connection);
            checkStatusCmd.Parameters.AddWithValue("@id", task.Id);
            
            var status = await checkStatusCmd.ExecuteScalarAsync();
            if (status == null)
            {
                return false;
            }
            
            var currentStatus = Convert.ToInt32(status);
            if (currentStatus == 2)
            {
                return false;
            }
            
            await using var updateCmd = new NpgsqlCommand(QueryConstants.Tasks.UpdateQuery, dbConn.Connection);
            updateCmd.Parameters.AddWithValue("@id", task.Id);
            updateCmd.Parameters.AddWithValue("@title", task.Title);
            updateCmd.Parameters.AddWithValue("@description", task.Description);
            updateCmd.Parameters.AddWithValue("@status", (int)task.Status);
            updateCmd.Parameters.AddWithValue("@priority", (int)task.Priority);
            updateCmd.Parameters.AddWithValue("@user_id", userId);

            var rowsAffected = await updateCmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        finally
        {
            await dbConn.Connection.CloseAsync();
        }
    }
}