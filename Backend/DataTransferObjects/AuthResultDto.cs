using TodoApp.Model;

namespace TodoApp.DataTransferObjects;

public class AuthResultDto
{
    public required string Token { get; set; }
    public required User User { get; set; }
}