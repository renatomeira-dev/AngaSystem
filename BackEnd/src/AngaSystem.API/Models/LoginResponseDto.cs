namespace AngaSystem.API.Models
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }
}
