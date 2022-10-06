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
        public async Task<ActionResult<Response<string>>> GenerateToken(User user)
        {
            Token_Generate generateToken = new Token_Generate(_context, _logger);
            string token = generateToken.GenerateToken(user);
            return Ok(token);
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<Response<string>>> RefreshToken(User user)
        {
            RefreshToken_Generate refreshToken = new RefreshToken_Generate(_context, _logger);
            (string key, string jwt) token = refreshToken.GenerateRefreshToken(user);
            return Ok(token.jwt);
        }
    }
}
