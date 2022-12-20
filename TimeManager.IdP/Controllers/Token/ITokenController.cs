using Microsoft.AspNetCore.Mvc;
using TimeManager.IdP.Data.Token;

namespace TimeManager.IdP.Authentication
{
    public interface ITokenController
    {
        public Task<IActionResult> VerifyToken(TokenDTO tokenDTO);
    }
}
