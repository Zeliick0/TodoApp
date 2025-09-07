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

namespace TodoApp.Managers;

public class UserManager(DbConn dbConn)
{
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
    
    public async Task<AuthResultDto?> RegisterUserAsync(AuthDto authDto)
    {
        await dbConn.Connection.OpenAsync();
        var checkCmd = new NpgsqlCommand(QueryConstants.Users.GetByUsernameQuery, dbConn.Connection);
        var existingUser = await checkCmd.ExecuteScalarAsync();
        if (existingUser != null)
        {
            return null;
        }
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(authDto.Password); 
        
        var command = new NpgsqlCommand(QueryConstants.Users.CreateQuery, dbConn.Connection);
        command.Parameters.AddWithValue("@username", authDto.Username);
        command.Parameters.AddWithValue("@password_hash", hashedPassword);
        
        var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var id  = reader.GetInt32(reader.GetOrdinal("id"));

        var user = new User
        {
            Id = id,
            Username = authDto.Username,
            PasswordHash = hashedPassword,
        };
        
        await dbConn.Connection.CloseAsync();
        var token = GenerateJwt(user);

        return new AuthResultDto
        {
            Token = token,
            User = user,
        };
    }

    public async Task<AuthResultDto?> LoginUserAsync(AuthDto authDto)
    {
        if (string.IsNullOrWhiteSpace(authDto.Username) || string.IsNullOrWhiteSpace(authDto.Password))
        {
            return null;
        }
        
        await dbConn.Connection.OpenAsync();
        var checkCmd = new NpgsqlCommand(QueryConstants.Users.GetByUsernameQuery, dbConn.Connection);
        var existingUser = await checkCmd.ExecuteReaderAsync();
        if (!await existingUser.ReadAsync())
        {
            return null;
        }

        var user = new User
        {
            Id = existingUser.GetInt32(existingUser.GetOrdinal("id")),
            Username = existingUser.GetString(existingUser.GetOrdinal("username")),
            PasswordHash = existingUser.GetString(existingUser.GetOrdinal("password_hash")),
        };
        
        bool isCorrectPassword =  BCrypt.Net.BCrypt.Verify(authDto.Password, user.PasswordHash);

        if (!isCorrectPassword)
        {
            return null;
        }
        
        var token =  GenerateJwt(user);
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }
        
        await dbConn.Connection.CloseAsync();
        return new AuthResultDto
        {
        Token = token,
        User = user,
        };
    }
    

    private static string GenerateJwt(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("MyAngelDamselette!123"!);

        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = "meow",
            Audience = "meow",
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}