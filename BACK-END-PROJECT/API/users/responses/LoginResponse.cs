namespace BACK_END_PROJECT.API.users.responses
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public UserResponse User { get; set; }

        // Construtor padrão
        public LoginResponse(string token, UserResponse user)
        {
            Token = token;
            User = user;
        }
    }
}
