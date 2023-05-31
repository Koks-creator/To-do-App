namespace WebApka.Models;

public class CreateUserDto
{
    public string Name {get; set;}
    public string LastName {get; set;}
    public string Username {get; set;}
    public string Password {get; set;}
    public string Email {get; set;}
    public int Age {get; set;}
}