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

                var result = processor.Execute(tokenDTO.token);
                
                return Ok(new Response<bool>(result));
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(new Response<bool>(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<bool>(ex));
            }
        }
    }
}
