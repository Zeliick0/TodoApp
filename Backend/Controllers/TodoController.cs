using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using TodoApp.Database;
using TodoApp.Helpers;
using TodoApp.Management;
using TodoApp.Model;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using Task = TodoApp.Model.Task;

namespace TodoApp.Controllers;

[ApiController]
[Route("tasks")]
public class TodoController(TaskManager taskManager) : ControllerBase
{
    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> GetListAsync()
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();
        
        var task = await taskManager.GetTasksByUserAsync(userId);
        return Ok(task);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTaskAsync([FromBody] Task task)
    {
        var create = await taskManager.CreateTaskAsync(task);
        return Ok(create);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateTaskAsync([FromBody] Task task)
    {
        var update = await taskManager.UpdateTaskAsync(task);
        return Ok(update);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTaskAsync([FromRoute]int id)
    {
        var delete = await taskManager.DeleteTaskAsync(id);
        return Ok(delete);
    }
}