using System.Buffers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApp.Database;
using TodoApp.Model;
using Npgsql;
using TodoApp.Constants;
using TodoApp.DataTransferObjects;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;

namespace TodoApp.Management;

public class UserManager(DbConn dbConn)
{
    public async Task<int> CreateUserAsync(User user)
    {
        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(QueryConstants.Users.CreateQuery, dbConn.Connection);
        
        command.Parameters.AddWithValue("@username", user.Username);
        command.Parameters.AddWithValue("@password_hash", user.PasswordHash);
        var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var id =  reader.GetInt32(reader.GetOrdinal("id"));
        
        await dbConn.Connection.CloseAsync();
        return id;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(QueryConstants.Users.DeleteQuery, dbConn.Connection);
        command.Parameters.AddWithValue("@id", id);
        
        await command.ExecuteNonQueryAsync();
        await dbConn.Connection.CloseAsync();
        return true;
    }
    
    public async Task<User?> GetUserByIdAsync(int id)
    {
        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(QueryConstants.Users.GetByIdQuery, dbConn.Connection);
        var reader = await command.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        
        var user = new User();
        user.Id = reader.GetInt32(reader.GetOrdinal("id"));
        user.Username = reader.GetString(reader.GetOrdinal("username"));
        user.PasswordHash = reader.GetString(reader.GetOrdinal("password_hash"));
        
        return user;
    }

    public async Task<bool> CheckUserByUsernameAsync(string username)
    {
        await dbConn.Connection.OpenAsync();
        var command = new NpgsqlCommand(QueryConstants.Users.GetByUsernameQuery, dbConn.Connection);
        
        command.Parameters.AddWithValue("@username", username);
        var reader = await command.ExecuteReaderAsync();
        
        var result =  await reader.ReadAsync();
        await dbConn.Connection.CloseAsync();
        
        return result;
    }
    
    public async Task<bool> RegisterUserAsync(AuthDto authDto)
    {
        await dbConn.Connection.OpenAsync();
        var checkCmd = new NpgsqlCommand(QueryConstants.Users.GetByUsernameQuery, dbConn.Connection);
        var existingUser = await checkCmd.ExecuteScalarAsync();
        if (existingUser != null)
        {
            return false;
        }
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(authDto.Password); 
        
        var command = new NpgsqlCommand(QueryConstants.Users.CreateQuery, dbConn.Connection);
        command.Parameters.AddWithValue("@username", authDto.Username);
        command.Parameters.AddWithValue("@password_hash", hashedPassword);
        
        await dbConn.Connection.CloseAsync();
        return true;
    }

    public async Task<AuthResultDto?> LoginUserAsync(AuthDto authDto)
    {
        if (string.IsNullOrWhiteSpace(authDto.Username) || string.IsNullOrWhiteSpace(authDto.Password))
        {
            return null;
        }
        
        await dbConn.Connection.OpenAsync();
        var checkCmd = new NpgsqlCommand(QueryConstants.Users.GetByUsernameQuery, dbConn.Connection);
        var existingUser = await checkCmd.ExecuteScalarAsync();
        if (existingUser == null)
        {
            return null;
        }

        
        return new AuthResultDto
        {
        Token = token,
        };
    }

    private Task<string> GenerateJWT(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes("meow"!);

        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = "",
            Audience = "",
        };
        
        
    }
}