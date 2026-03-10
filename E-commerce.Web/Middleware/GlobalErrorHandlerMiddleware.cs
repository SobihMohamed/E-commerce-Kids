using E_commerce.Shared.Common.Responses;
using System.Text.Json;

namespace E_commerce.Web.Middleware
{
    public class GlobalErrorHandlerMiddleware
        // nextMw is the next middleware in the pipeline, logger is used for logging exceptions
        (RequestDelegate nextMW , ILogger<GlobalErrorHandlerMiddleware> logger)
    {
        // The InvokeAsync method is called for each HTTP request.
        // It tries to execute the next middleware and checks for 404 status code.
        // because 404 is not an exception, it won't be caught in the catch block,
        // all he does is check if the response status code is 404 and returns it to front without any reasons or msg
        // so we need to check for it explicitly after the next middleware has executed.
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await nextMW(context);
                // sometime the server can start sending the header of response to the client before we check the status code,
                // so we need to check if the response has started or not before we try to modify it.
                // because if the response has already started, we can't modify the status code or the content type, and we can't send a new response body.
                // check has started is used to ensure that server hasn't already sent a response to the client, because if we have, we can't modify the response anymore.
                if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
                {
                    await HandelResponseAsync(context, 404, "The requested resource was not found.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error : {ex.Message}");
                await HandelExceptionAsync(context, ex);
            }
        }
        // HandelResponseAsync is a helper method to send a consistent JSON response to the client,
        // it takes the HttpContext, status code, and message as parameters.
        public async Task HandelResponseAsync(HttpContext context, int statusCode, string msg)
        {
            context.Response.ContentType = "application/json"; // change from html to json because we want to return a json response to the client
            context.Response.StatusCode = statusCode;

            var response = new ApiResponse<string>(msg,statusCode); // wrapper design pattern to ensure consistent response structure across the application, even for error responses. 
            // convert from object ApiJson to json string to send it to the client,
            // and we use camelCase naming policy to ensure that the property names in the JSON response are in camelCase format,
            // which is a common convention in JavaScript and JSON data.
            var json = JsonSerializer.Serialize(
                response, 
                new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
             );
            await context.Response.WriteAsync(json);
        }

        public async Task HandelExceptionAsync(HttpContext context, Exception ex)
        {
        }
    }
}
