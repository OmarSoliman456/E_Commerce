using E_Commerce.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        protected static ActionResult<T> ToActionResult<T>(Result result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result);
            }
            else
            {
                return ToProblem(result.Errors);
            }
        }

        protected static ActionResult<T> ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value);
            }
            else
            {
                return ToProblem(result.Errors);
            }
        }

        protected static ObjectResult ToProblem(IReadOnlyList<Error> errors)
        {
            var firstError = errors[0];

            var statusCode = firstError.ErrorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError,
            };

            var problem = new ProblemDetails()
            {
                Detail = firstError.Description,
                Title = firstError.Code,
                Status = statusCode,
                Extensions = { ["Errors"] = errors }
            };

            return new ObjectResult(problem)
            {
                StatusCode = statusCode
            };
        }

    }
}
