using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using TodoApp.Database;
using TodoApp.Helpers;
using TodoApp.Managers;
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
        
        var task = await taskManager.GetTasksByUserAsync(userId.Value);
        return Ok(task);
    }
    
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateTaskAsync([FromBody] Task task)
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();
        
        var create = await taskManager.CreateTaskAsync(task, userId.Value);
        return Ok(create);
    }
    
    [HttpPost("update")]
    [Authorize]
    public async Task<IActionResult> UpdateTaskAsync([FromBody] Task task)
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();
        
        var update = await taskManager.UpdateTaskAsync(task, userId.Value);
        return Ok(update);
    }
    
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteTaskAsync([FromRoute]int id)
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();
        
        var delete = await taskManager.DeleteTaskAsync(id, userId.Value);
        return Ok(delete);
    }
}