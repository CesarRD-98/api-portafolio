using Cesardd.Core.Features.Contact.SendContact;
using Cesardd.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Cesardd.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("RateLimiterPolicy")]
    public class ContactController(SendContactHandler handler) : ControllerBase
    {
        private readonly SendContactHandler _handler = handler;

        [HttpPost]
        public async Task<IActionResult> Send(SendContactRequest request)
        {
            var result = await _handler.Handle(request);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.Fail(result.Error!));
            }

            return Ok(ApiResponse<SendContactResponse>.Ok(result.Value!));
        }
    }
}
