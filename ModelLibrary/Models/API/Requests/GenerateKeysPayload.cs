namespace ModelLibrary.Models.API.Requests
{
    public class GenerateKeysPayload
    {
        
        public int Amount { get; set; }
        public int Type { get; set; }
        
        public int Duration { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        
    }
}