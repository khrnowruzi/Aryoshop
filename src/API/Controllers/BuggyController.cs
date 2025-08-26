using System.Security.Claims;
using Application.DTOs.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        [HttpGet("unauthorized")]
        public IActionResult GetUnAuthorized()
        {
            return Unauthorized();  //Status401Unauthorized
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("Not a good request");    //Status400BadRequest
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            return NotFound();  //Status404NotFound
        }

        [HttpGet("internalerror")]
        public IActionResult GetInternalError()
        {
            throw new Exception("This is a test exception of a internall server error!");
        }

        [HttpPost("validationerror")]
        public IActionResult GetValidationError(ProductDto produtDto)
        {
            return Ok();    //Status400BadRequest, response with validation errors
        }

        [Authorize]
        [HttpGet("secretinfo")]
        public IActionResult GetSecretInfo()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok("User name is " + name + " and user id is " + userId);
        }
    }
}
