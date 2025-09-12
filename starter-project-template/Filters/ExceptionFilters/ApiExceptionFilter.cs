using Domain.CustomExceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

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
                case IdentityException:
                case ValidationException:
                case NullParameterException:
                case NotAllowedException:
                    statusCode = 400;
                    break;

                case InvalidCredentialsException:
                case InvalidAccessTokenException:
                case TwoFactorAuthFailedException:
                    statusCode = 401;
                    break;

                case NotConfirmedException:
                    statusCode = 403;
                    break;

                case ObjectNotFoundException:
                    statusCode = 404;
                    break;

                case ConflictException:
                    statusCode = 409;
                    break;

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