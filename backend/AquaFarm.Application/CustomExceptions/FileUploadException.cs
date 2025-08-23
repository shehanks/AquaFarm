namespace AquaFarm.Application.CustomExceptions
{
    public class FileUploadException : AquaFarmException
    {
        public FileUploadException(string errorCode, int statusCode, string message)
            : base(errorCode, statusCode, message)
        {
        }

        public FileUploadException(string action, int statusCode, string message, Exception innerException)
            : base(action, statusCode, message, innerException)
        {
        }
    }
}
