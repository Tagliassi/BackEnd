namespace BACK_END_PROJECT.API.roles.responses
{
    public class RoleResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // Construtor que recebe uma Role e mapeia para RoleResponse
        public RoleResponse(Role role)
        {
            Name = role.Name;
            Description = role.Description;
        }
    }
}
