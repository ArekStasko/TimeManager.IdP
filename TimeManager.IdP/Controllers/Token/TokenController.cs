using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data;
using TimeManager.IdP.Processors.TokenProcessor;
using TimeManager.IdP.Data.Token;

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

        [HttpPost("verifyToken")]
        public async Task<ActionResult<Response<bool>>> VerifyToken(TokenDTO tokenDTO)
        {
            try
            {
                Token_Verify verifyToken = new Token_Verify(_context, _logger);
                bool userId = verifyToken.VerifyToken(tokenDTO.token);

                return Ok(new Response<bool>(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"PROCESS FAILED: {ex.Message}  || TOKEN: {tokenDTO.token}");
                return BadRequest(ex.Message);
            }
        }
    }
}
