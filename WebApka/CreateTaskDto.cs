namespace WebApka.Models;

public class CreateTaskDto
{
    public string Title {get; set;}
    public string Content {get; set;}
    public DateTime EndTime { get; set; }
    public int UserId { get; set; }
}