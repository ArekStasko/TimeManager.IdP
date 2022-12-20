using Microsoft.AspNetCore.Mvc;
using TimeManager.IdP.Data;
using Microsoft.AspNetCore.Authorization;
using TimeManager.IdP.services;

namespace TimeManager.IdP.Authentication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly IProcessors _processors;
        public AuthController(IProcessors processors) => _processors = processors;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO request)
        {
            var processor = _processors.user_Register;
            if (processor == null) return BadRequest(new ArgumentNullException(nameof(processor)));

            var result = await processor.Execute(request);

            return result.Match<IActionResult>(token =>
            {
                return CreatedAtAction("Post", token);
            }, exception =>
            {
                return BadRequest(exception);
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO request)
        {
            var processor = _processors.user_Login;
            if(processor == null) return BadRequest(new ArgumentNullException(nameof(processor)));

            var result = await processor.Execute(request);

            return result.Match<IActionResult>(token =>
            {
                return CreatedAtAction("Post", token);
            }, exception =>
            {
                return BadRequest(exception);
            });
        }
    }
}
