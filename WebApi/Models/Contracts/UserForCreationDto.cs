namespace WebApi.Models.Contracts
{
    public class UserForCreationDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Password_Confirm { get; set; }
    }
}
