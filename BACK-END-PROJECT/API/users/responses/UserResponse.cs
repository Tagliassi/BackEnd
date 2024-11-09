namespace BACK_END_PROJECT.API.users.responses
{
    public class UserResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Construtor padrão
        public UserResponse(long id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        // Construtor que aceita um User e inicializa o UserResponse
        public UserResponse(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
        }
    }
}
