using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data;
using TimeManager.IdP.Processors.TokenProcessor;

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

        [HttpPost("register")]
        public async Task<ActionResult<Response<User>>> Register(UserDTO request)
        {
            User_Register register = new User_Register(_context, _logger);
            var User = register.Register(request);
            return Ok(User);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<string>>> Login(UserDTO request)
        {
            User_Login login = new User_Login(_context, _logger);
            var User = login.Login(request);            
            return Ok(User);
        }
    }
}
