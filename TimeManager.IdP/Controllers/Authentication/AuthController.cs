using Microsoft.AspNetCore.Mvc;
using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data;
using TimeManager.IdP.Processors.UserProcessor;
using Microsoft.AspNetCore.Authorization;
using TimeManager.IdP.Data.Token;
using TimeManager.IdP.services;

namespace TimeManager.IdP.Authentication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly IUserProcessors _processors;
        public AuthController(IUserProcessors processors) => _processors = processors;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<Response<TokenDTO>>> Register(UserDTO request)
        {
            return Ok(_processors.Register(request));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<Response<TokenDTO>>> Login(UserDTO request)
        {
            return Ok(_processors.Login(request));
        }
    }
}
