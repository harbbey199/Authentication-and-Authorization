using Contracts;
using Credential_Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Paylode_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthentication _authentication;

        public AuthController(IAuthentication authentication)
        {
            _authentication = authentication;
        }
        [HttpPost("login")]
        public async Task< IActionResult> Login([FromBody] LoginModel user)
        {
            
            if (user is null)
            {
                return BadRequest("Invalid client request");
            }
            var result = await _authentication.Login(user);
            return Ok(new AuthenticatedResponse { Token=result});
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return BadRequest("Invalid Input");
            }
            var registration = await _authentication.Register(registerDto);
            return Ok($"successfully registered{registration}");
        }
    }
}
