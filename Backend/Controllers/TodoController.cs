using Microsoft.AspNetCore.Mvc;
using TodoApp.Database;
using TodoApp.Management;
using TodoApp.Model;

namespace TodoApp.Controllers;

[ApiController]
[Route("tasks")]
public class TodoController(TaskManager taskManager) : ControllerBase
{
    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync()
    {
        var task = await taskManager.GetTasksAsync();
        return Ok(task);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTaskAsync([FromBody] Tasks task)
    {
        var create = await taskManager.CreateTaskAsync(task);
        return Ok(create);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateTaskAsync([FromBody] Tasks task)
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