using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApka.Models;

public class TaskModel
{   
    public int id {get; set;}

    [Required(ErrorMessage = "Please enter a title")]
    public string Title {get; set;}

    [Required(ErrorMessage = "Please enter a content")]
    public string Content {get; set;}

    
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Please enter a end date")]
    public DateTime  EndTime { get; set; }

    public string Status { get; set; } = "Created";

    [JsonIgnore]
    public UserModel User { get; set; }

    public int UserId { get; set; }

}
