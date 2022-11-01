using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data;
using TimeManager.IdP.Processors.TokenProcessor;
using Microsoft.AspNetCore.Authorization;
using TimeManager.IdP.Data.Token;

namespace TimeManager.IdP.Authentication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly DataContext _context;
        private readonly ILogger<TokenController> _logger;

        public AuthController(DataContext context, ILogger<TokenController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<Response<TokenDTO>>> Register(UserDTO request)
        {
            User_Register register = new User_Register(_context, _logger);
            Response<TokenDTO> token = register.Register(request);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<Response<TokenDTO>>> Login(UserDTO request)
        {
            User_Login login = new User_Login(_context, _logger);
            Response<TokenDTO> token = login.Login(request);            
            return Ok(token);
        }
    }
}
