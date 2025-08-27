using Npgsql;
using TodoApp.Database;

namespace TodoApp.Management;

public class DeleteTask(DbConn dbConn)
{
    private const string DeleteQuery = "DELETE FROM tasks WHERE id = @id";

    public async Task<bool> DeleteTaskAsync(int id)
    {
        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(DeleteQuery, dbConn.Connection);
        
        command.Parameters.AddWithValue("@id", id);
        await  command.ExecuteNonQueryAsync();
        await dbConn.Connection.CloseAsync();
        return true;
    }
}