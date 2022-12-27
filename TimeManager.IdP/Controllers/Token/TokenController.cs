using Microsoft.AspNetCore.Mvc;
using TimeManager.IdP.Data.Token;
using TimeManager.IdP.services;

namespace TimeManager.IdP.Authentication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase, ITokenController
    {
        private readonly IProcessors _processors;
        public TokenController(IProcessors processors) => _processors = processors;

        [HttpPost("verifyToken")]
        public async Task<IActionResult> VerifyToken(TokenDTO tokenDTO)
        {
            var processor = _processors.token_Verify;
            if(processor == null) return BadRequest(new ArgumentNullException(nameof(processor)));

            var result = await processor.Execute(tokenDTO.token);

            return result.Match<IActionResult>(success =>
            {
                return CreatedAtAction(nameof(VerifyToken), success);
            }, exception =>
            {
                return BadRequest(exception);
            });
        }
    }
}
