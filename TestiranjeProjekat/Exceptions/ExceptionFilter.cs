using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TestiranjeProjekat.Exceptions;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var result = context.Exception switch
        {
            ExistingPlayerException existingPlayerException => new ObjectResult(new
            {
                message = existingPlayerException.Message
            })
            {
                StatusCode = existingPlayerException.StatusCode
            },
            EmptyFieldException emptyFieldException => new ObjectResult(new
            {
                message = emptyFieldException.Message
            })
            {
                StatusCode = emptyFieldException.StatusCode
            },
            NonExistingIdException nonExistingIdException => new ObjectResult(new
            {
                message = nonExistingIdException.Message
            })
            {
                StatusCode = nonExistingIdException.StatusCode
            },
            _ => new ObjectResult(new
            {
                message = "An unexpected error occurred."
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };

        context.Result = result;
        context.ExceptionHandled = true; // Obeleži da je izuzetak obrađen
    }
}
