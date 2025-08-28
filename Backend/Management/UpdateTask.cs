using Npgsql;
using TodoApp.Database;
using TodoApp.Model;

namespace TodoApp.Management;

public class UpdateTask(DbConn dbConn)
{
    private const string UpdateQuery = @"
        UPDATE tasks 
        SET title = @title, description = @description, status = @status, priority = @priority 
        WHERE id = @id";

    private const string CheckQuery = "SELECT status FROM tasks WHERE id = @id";

    public async Task<bool> UpdateTaskAsync(Tasks task)
    {
        await dbConn.Connection.OpenAsync();

        try
        {
            using var checkCmd = new NpgsqlCommand(CheckQuery, dbConn.Connection);
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

            await using var updateCmd = new NpgsqlCommand(UpdateQuery, dbConn.Connection);
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