using Microsoft.AspNetCore.Mvc;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.Data;
using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data.Token;
using TimeManager.IdP.Processors.TokenProcessor;

namespace TimeManager.IdP.services
{
    public class TokenProcessors : ITokenProcessors
    {
        private readonly DataContext _context;
        private readonly ILogger<TokenController> _logger;

        public TokenProcessors(DataContext context, ILogger<TokenController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActionResult<Response<bool>>> VerifyToken(TokenDTO tokenDTO)
        {
            Token_Verify verifyToken = new Token_Verify(_context, _logger);
            var userId = verifyToken.VerifyToken(tokenDTO.token);
            return new Response<bool>(userId);
        }

    }
}
