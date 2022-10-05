using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data;
using TimeManager.IdP.Processors.TokenProcessor;

namespace TimeManager.IdP.Authentication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase, ITokenController
    {
        private readonly DataContext _context;
        private readonly ILogger<TokenController> _logger;

        public TokenController(DataContext context, ILogger<TokenController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("generateToken")]
        public async Task<ActionResult<Response<string>>> GenerateToken(Token request)
        {
            User_IsAuth IsAuth = new User_IsAuth(_context, _logger);
            var User = IsAuth.IsAuthorised(request);
            return Ok(User);
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<Response<string>>> RefreshToken(Token request)
        {
            User_IsAuth IsAuth = new User_IsAuth(_context, _logger);
            var User = IsAuth.IsAuthorised(request);
            return Ok(User);
        }
    }
}
