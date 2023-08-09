using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Startup.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public GlobalExceptionFilter()
        {
        }

        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                var exception = context.Exception;
                var statusCode = true switch
                {
                    bool when exception is ArgumentNullException => StatusCodes.Status404NotFound,
                    bool when exception is NullReferenceException => StatusCodes.Status404NotFound,
                    bool when exception is InvalidOperationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError,
                };

                context.Result = new ContentResult() 
                {
                    Content = exception.Message,
                    StatusCode = statusCode
                };
            }
        }
    }
}
