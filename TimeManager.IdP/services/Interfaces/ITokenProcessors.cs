using Microsoft.AspNetCore.Mvc;
using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data.Token;

namespace TimeManager.IdP.services
{
    public interface ITokenProcessors
    {
        public Task<ActionResult<Response<bool>>> VerifyToken(TokenDTO tokenDTO);
    }
}
