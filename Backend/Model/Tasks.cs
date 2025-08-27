namespace TodoApp.Model;

public class Tasks
{
    public int  Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; }
   
    public Priority Priority { get; set; }
    
    public DateTime CreatedAt { get; set; }
}