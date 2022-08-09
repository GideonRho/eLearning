namespace WebAPI.Models.API.Requests
{
    public class SetChoice
    {
        /// <example>3</example>
        public int Id { set; get; }
        /// <example>1</example>
        public int Choice { set; get; }

        public SetChoice()
        {
        }

        public SetChoice(int id, int choice)
        {
            Id = id;
            Choice = choice;
        }
    }
}