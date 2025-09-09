using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace starter_project_template.Filters.ExceptionFilters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string message = context.Exception.Message;
            int statusCode = 500;

            switch (context.Exception)
            {
                default:
                    statusCode = 500;
                    break;
            }

            context.Result = new ObjectResult(new { error = message })
            {
                StatusCode = statusCode
            };
            context.ExceptionHandled = true;
        }
    }
}