using Backend.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TestiranjeProjekat.Exceptions;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var result = context.Exception switch
        {
            EmptyFieldException emptyFieldException => new ObjectResult(new
            {
                message = emptyFieldException.Message
            })
            {
                StatusCode = emptyFieldException.StatusCode
            },
            EmptyTournamentListException emptyTournamentListException => new ObjectResult(new
            {
                message = emptyTournamentListException.Message
            })
            {
                StatusCode = emptyTournamentListException.StatusCode
            },
            ExistingOrganizatorException existingOrganizatorException => new ObjectResult(new
            {
                message = existingOrganizatorException.Message
            })
            {
                StatusCode = existingOrganizatorException.StatusCode
            },
            ExistingPlayerException existingPlayerException => new ObjectResult(new
            {
                message = existingPlayerException.Message
            })
            {
                StatusCode = existingPlayerException.StatusCode
            },
            ExistingTournamentException existingTournamentException => new ObjectResult(new
            {
                message = existingTournamentException.Message
            })
            {
                StatusCode = existingTournamentException.StatusCode
            },
            FullTournamentCapacityException fullTournamentCapacityException => new ObjectResult(new
            {
                message = fullTournamentCapacityException.Message
            })
            {
                StatusCode = fullTournamentCapacityException.StatusCode
            },
            NonExistingIdException nonExistingIdException => new ObjectResult(new
            {
                message = nonExistingIdException.Message
            })
            {
                StatusCode = nonExistingIdException.StatusCode
            },
            NonExistingOrganizatorException nonExistingOrganizatorException => new ObjectResult(new
            {
                message = nonExistingOrganizatorException.Message
            })
            {
                StatusCode = nonExistingOrganizatorException.StatusCode
            },
            NonExistingPlayerException nonExistingPlayerException => new ObjectResult(new
            {
                message = nonExistingPlayerException.Message
            })
            {
                StatusCode = nonExistingPlayerException.StatusCode
            },
            NonExistingRegistrationException nonExistingRegistrationException => new ObjectResult(new
            {
                message = nonExistingRegistrationException.Message
            })
            {
                StatusCode = nonExistingRegistrationException.StatusCode
            },
            NonExistingTeamLeaderException nonExistingTeamLeaderException => new ObjectResult(new
            {
                message = nonExistingTeamLeaderException.Message
            })
            {
                StatusCode = nonExistingTeamLeaderException.StatusCode
            },
            NonExistingTournamentException nonExistingTournamentException => new ObjectResult(new
            {
                message = nonExistingTournamentException.Message
            })
            {
                StatusCode = nonExistingTournamentException.StatusCode
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
