using Npgsql;
using TodoApp.Database;
using TodoApp.Model;

namespace TodoApp.Management;

public class Update(DbConn dbConn)
{
    private const string UpdateQuery = "UPDATE tasks SET title = @title, description = @description, status = @status WHERE id = @id";

    public async Task<bool> UpdateTaskAsync(int id,  string title, string description, Status status)
    {
        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(UpdateQuery, dbConn.Connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@title", title);
        command.Parameters.AddWithValue("@description", description);
        command.Parameters.AddWithValue("@status", (int)status);
        await command.ExecuteNonQueryAsync();

        await dbConn.Connection.CloseAsync();
        return true;
    }
}