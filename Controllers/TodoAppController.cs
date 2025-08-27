using Microsoft.AspNetCore.Mvc;
using TodoApp.Database;
using TodoApp.Management;
using TodoApp.Model;

namespace TodoApp.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoAppController(DbConn dbConn) : ControllerBase
{
    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync()
    {
        var getTasks = new GetTasks(dbConn);
        var tasks = await getTasks.GetTasksAsync();
        return Ok(tasks);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTaskAsync([FromBody] Tasks task)
    {
        var createTask = new CreateTask(dbConn);
        var create = await createTask.CreateTaskAsync(task);
        return Ok(create);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateTaskAsync([FromBody] Tasks task)
    {
        var updateTask = new UpdateTask(dbConn);
        var update = await updateTask.UpdateTaskAsync(task);
        return Ok(update);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTaskAsync([FromRoute]int id)
    {
        var deleteTask = new DeleteTask(dbConn);
        var delete = await deleteTask.DeleteTaskAsync(id);
        return Ok(delete);
    }
}