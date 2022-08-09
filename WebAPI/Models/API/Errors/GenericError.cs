namespace WebAPI.Models.API.Errors
{
    
    /// <summary>
    /// Represents a error that happened while executing a http request.
    /// See the status code descriptions of the responsible endpoint for more information.
    /// </summary>
    public class GenericError
    {

        public string Message { get; set; }

        public GenericError(string message)
        {
            Message = message;
        }
    }
}