namespace WebApplicationMV.API.Models.AuthenticationModels
{
    public class AuthenticationDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
