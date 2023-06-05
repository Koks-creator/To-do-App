using System.ComponentModel.DataAnnotations;

namespace WebApka.Models;

public class CreateTaskDto
{   
    [Required(ErrorMessage = "Please enter a title")]
    public string Title {get; set;}

    [Required(ErrorMessage = "Please enter a content")]
    public string Content {get; set;}

    [Required(ErrorMessage = "Please enter a end time")]
    public DateTime EndTime { get; set; }
    public int UserId { get; set; }
}