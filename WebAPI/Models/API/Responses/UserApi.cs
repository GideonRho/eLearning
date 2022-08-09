namespace WebAPI.Models.API.Responses
{
    public class UserApi
    {
        
        /// <example>3</example>
        public int Id { get; set; }
        /// <example>Hans</example>
        public string Name { get; set; }
        public bool EmailConfirmed { get; set; }

        public UserApi()
        {
        }

        public UserApi(int id, string name, bool emailConfirmed)
        {
            Id = id;
            Name = name;
            EmailConfirmed = emailConfirmed;
        }
    }
}