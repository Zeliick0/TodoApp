using Microsoft.AspNetCore.Mvc;
using TodoApp.DataTransferObjects;
using TodoApp.Managers;
using TodoApp.Model;
using Task = System.Threading.Tasks.Task;

namespace TodoApp.Controllers;

[ApiController]
[Route("users")]
public class UserController(UserManager userManager) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDto authDto)
    {
        var register = await userManager.RegisterUserAsync(authDto);
        return Ok(register);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthDto authDto)
    {
        var login = await userManager.LoginUserAsync(authDto);
        return Ok(login);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new { message = "Logout successful" });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var delete = await userManager.DeleteUserAsync(id);
        return Ok(delete);
    }
}