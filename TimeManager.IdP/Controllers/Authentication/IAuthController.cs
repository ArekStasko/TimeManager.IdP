using Microsoft.AspNetCore.Mvc;
using TimeManager.IdP.Data;

namespace TimeManager.IdP.Authentication
{
    public interface IAuthController
    {
        public Task<IActionResult> Register(UserDTO request);
        public Task<IActionResult> Login(UserDTO request);
    }
}
