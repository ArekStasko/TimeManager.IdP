using Microsoft.AspNetCore.Mvc;
using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data;
using TimeManager.IdP.Processors.UserProcessor;
using Microsoft.AspNetCore.Authorization;
using TimeManager.IdP.Data.Token;
using TimeManager.IdP.services;

namespace TimeManager.IdP.Authentication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly IProcessors _processors;
        public AuthController(IProcessors processors) => _processors = processors;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<Response<TokenDTO>>> Register(UserDTO request)
        {
            try
            {
                var processor = _processors.user_Register;
                if (processor == null) throw new ArgumentNullException(nameof(processor));

                var token = processor.Execute(request);
                return Ok(new Response<TokenDTO>(token));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new Response<TokenDTO>(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<TokenDTO>(ex));

            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<Response<TokenDTO>>> Login(UserDTO request)
        {
            try
            {
                var processor = _processors.user_Login;
                if(processor == null) throw new ArgumentNullException(nameof(processor));

                var token = processor.Execute(request);
                return Ok(new Response<TokenDTO>(token));
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(new Response<TokenDTO>(ex));
            }
            catch(Exception ex)
            {
                return BadRequest(new Response<TokenDTO>(ex));
            }
        }
    }
}
