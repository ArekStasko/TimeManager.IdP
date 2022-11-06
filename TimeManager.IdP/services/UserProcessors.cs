using Microsoft.AspNetCore.Mvc;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.Data;
using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data.Token;
using TimeManager.IdP.Processors.UserProcessor;

namespace TimeManager.IdP.services
{
    public class UserProcessors : IUserProcessors
    {
        private readonly DataContext _context;
        private readonly ILogger<TokenController> _logger;
        public UserProcessors(DataContext context, ILogger<TokenController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ActionResult<Response<TokenDTO>>> Register(UserDTO request)
        {
            User_Register register = new User_Register(_context, _logger);
            return register.Register(request);
        }

        public async Task<ActionResult<Response<TokenDTO>>> Login(UserDTO request)
        {
            User_Login login = new User_Login(_context, _logger);
            return login.Login(request);
        }

    }
}
