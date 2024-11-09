namespace BACK_END_PROJECT.API.users.requests
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        // Construtor padrão (opcional, caso precise)
        public LoginRequest() { }

        // Construtor para inicializar com email e senha
        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
