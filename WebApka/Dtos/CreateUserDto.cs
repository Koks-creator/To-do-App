using System.ComponentModel.DataAnnotations;

namespace WebApka.Models;


public class CreateUserDto
{
    [Required(ErrorMessage = "Please enter a name")]
    public string Name {get; set;}

    [Required(ErrorMessage = "Please enter a last name")]
    public string LastName {get; set;}

    [Required(ErrorMessage = "Please enter a username")]
    [StringLength(40)]
    public string Username {get; set;}

    [Required(ErrorMessage = "Please enter a password")]
    public string Password {get; set;}

    [Required(ErrorMessage = "Please enter an email")]
    [EmailAddress]
    public string Email {get; set;}

    [Range(1, 200)] // allowed range of values for age
    [Required(ErrorMessage = "Age has to be between 16 and 200")]
    public int Age {get; set;}
}