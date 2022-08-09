namespace WebAPI.Models.API.Responses
{
    public class TagApi
    {
        
        public string Name { get; set; }

        public TagApi()
        {
        }

        public TagApi(string name)
        {
            Name = name;
        }
    }
}