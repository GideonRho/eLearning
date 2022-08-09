namespace WebAPI.Models.API.Requests
{
    public class ImageFilepond
    {
        
        public string ReferenceKey { get; set; }
        public bool LinkCourse { get; set; }
        public bool LinkQuestion { get; set; }
        public bool LinkComment { get; set; }
        public bool LinkAnswer { get; set; }

        public ImageFilepond()
        {
        }
    }
}