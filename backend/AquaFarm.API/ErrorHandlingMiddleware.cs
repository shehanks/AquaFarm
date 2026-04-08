using AquaFarm.Application;
using AquaFarm.Application.CustomExceptions;
using AquaFarm.Application.DTOs;
using System.Net;

namespace AquaFarm.API
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var httpErrorCode = HttpStatusCode.InternalServerError;
                var errorResponse = new ErrorResponse();

                if (ex is AquaFarmException)
                {
                    var aquaEx = (AquaFarmException)ex;

                    logger.LogError(aquaEx, "Aqua Farm Exception log: exceptionType: {@exceptionType}, action: {@action}, statusCode: {@statusCode}",
                    "General", aquaEx.Action, aquaEx.StatusCode);

                    errorResponse.ErrorMessage = "Unknown error while trying to call the service";
                    errorResponse.ErrorCode = ErrorCodes.InternalError;
                }
                else
                {
                    logger.LogError(ex, "Unhandled error. {@errorType}", "General");
                    errorResponse.ErrorMessage = "Unknown error";
                    errorResponse.ErrorCode = ErrorCodes.InternalError;
                }

                context.Response.StatusCode = (int)httpErrorCode;
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }

    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
