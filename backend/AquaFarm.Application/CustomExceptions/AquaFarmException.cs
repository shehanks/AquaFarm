namespace AquaFarm.Application.CustomExceptions
{
    public class AquaFarmException : Exception
    {
        public string Action { get; private set; }

        public int StatusCode { get; private set; }

        public AquaFarmException(string errorCode, int statusCode, string message)
            : base(message)
        {
            Action = errorCode;
            StatusCode = statusCode;
        }

        public AquaFarmException(string action, int statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            Action = action;
            StatusCode = statusCode;
        }
    }
}
