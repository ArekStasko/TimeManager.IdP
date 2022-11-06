using Microsoft.AspNetCore.Mvc;
using TimeManager.IdP.Data;
using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data.Token;

namespace TimeManager.IdP.services
{
    public interface IUserProcessors
    {
        public Task<ActionResult<Response<TokenDTO>>> Register(UserDTO request);
        public Task<ActionResult<Response<TokenDTO>>> Login(UserDTO request);
    }
}
