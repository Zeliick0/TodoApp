using TodoApp.Database;
using Npgsql;
using TodoApp.Model;
using TodoApp.Constants;

namespace TodoApp.Management;

public class TaskManager(DbConn dbConn)
{
    public async Task<int> CreateTaskAsync(Tasks task)
    {
        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(QueryConstants.Tasks.AddQuery, dbConn.Connection);
        
        command.Parameters.AddWithValue("@title", task.Title);
        command.Parameters.AddWithValue("@description", task.Description);
        command.Parameters.AddWithValue("@status", 0);
        command.Parameters.AddWithValue("@priority", (int)task.Priority);
        var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var id = reader.GetInt32(reader.GetOrdinal("id"));
        
        await  dbConn.Connection.CloseAsync();
        return id;
    }
    
    public async Task<bool> DeleteTaskAsync(int id)
    {
        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(QueryConstants.Tasks.DeleteQuery, dbConn.Connection);
        
        command.Parameters.AddWithValue("@id", id);
        await  command.ExecuteNonQueryAsync();
        await dbConn.Connection.CloseAsync();
        return true;
    }

    public async Task<List<Tasks>> GetTasksAsync()
    {
        var tasks = new List<Tasks>();

        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(QueryConstants.Tasks.GetQuery, dbConn.Connection);
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

    public async Task<bool> UpdateTaskAsync(Tasks task)
    {
        await dbConn.Connection.OpenAsync();

        try
        {
            await using var checkCmd = new NpgsqlCommand(QueryConstants.Tasks.CheckQuery, dbConn.Connection);
            checkCmd.Parameters.AddWithValue("@id", task.Id);

            var statusObj = await checkCmd.ExecuteScalarAsync();
            if (statusObj == null)
            {
                return false;
            }

            var currentStatus = Convert.ToInt32(statusObj);

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

            var rowsAffected = await updateCmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        finally
        {
            await dbConn.Connection.CloseAsync();
        }
    }
}