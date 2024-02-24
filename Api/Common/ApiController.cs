using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Common;

[ApiController]
public class ApiController : ControllerBase
{
    protected IResult Problem(IEnumerable<IError> errors)
    {
        return Results.Problem(
            statusCode: StatusCodes.Status400BadRequest,
            detail: errors.First().Message
        );
    }
}
