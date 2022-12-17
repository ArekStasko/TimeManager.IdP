using Microsoft.AspNetCore.Mvc;
using TimeManager.IdP.Data.Response;
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
        public async Task<ActionResult<Response<bool>>> VerifyToken(TokenDTO tokenDTO)
        {
            try
            {
                var processor = _processors.token_Verify;

                if(processor == null) throw new ArgumentNullException(nameof(processor));

                return Ok(processor.Execute(tokenDTO.token));
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
