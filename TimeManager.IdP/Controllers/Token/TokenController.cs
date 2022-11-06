using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data;
using TimeManager.IdP.Processors.TokenProcessor;
using TimeManager.IdP.Data.Token;
using TimeManager.IdP.services;

namespace TimeManager.IdP.Authentication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase, ITokenController
    {
        private readonly ITokenProcessors _processors;
        public TokenController(ITokenProcessors processors) => _processors = processors;

        [HttpPost("verifyToken")]
        public async Task<ActionResult<Response<bool>>> VerifyToken(TokenDTO tokenDTO)
        {
            try
            {
                return Ok(_processors.VerifyToken(tokenDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
