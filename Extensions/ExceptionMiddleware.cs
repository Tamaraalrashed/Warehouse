using Data;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;
using static Data.AppException;

public static class ExceptionMiddleware
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var appResponse = new HTTPErrorResponse(BusinessErrorCodes.ServerError)
                {
                    ErrorMessageCode = "An unexpected error occurred."
                };
                var statusCode = (int)HttpStatusCode.InternalServerError;

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    if (contextFeature.Error is AppException appException)
                    {
                        appResponse.ErrorCode = appException.Code;
                        appResponse.ErrorMessageCode = appException.ErrorMessage;
                        statusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else
                    {
                        appResponse.ErrorMessageCode = contextFeature.Error.Message;
                    }
                }

                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(appResponse));
            });
        });
    }
}
