using Microsoft.AspNetCore.Mvc;
using PeopleIncApi.Interfaces;

namespace PeopleIncApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public TokenController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult GenerateToken()
        {
            try
            {
                var token = _jwtService.GenerateToken();
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to generate token: {ex.Message}");
            }
        }
    }
}
