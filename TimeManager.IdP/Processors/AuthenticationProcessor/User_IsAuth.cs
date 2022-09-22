using TimeManager.IdP.Data.Response;
using System.Security.Cryptography;
using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;

namespace TimeManager.IdP.Processors.AuthenticationProcessor
{
    public class User_IsAuth : Processor
    {
        public User_IsAuth(DataContext _context, ILogger<AuthController> logger) : base(_context, logger) {}
        public Response<bool> IsAuthorised(Token data)
        {
            Response<bool> response;
            try
            {
                var user_token = new User_Token(_context, _logger);

                if (!_context.Tokens.Any(t => t.token == data.token) || !user_token.CheckExpirationDate(data))
                {
                    throw new Exception("User is not authenticated");
                }

                response = new Response<bool>(true);
                _logger.LogInformation("User successfully authenticated");
                return response;
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new Response<bool>(ex, "Whoops something went wrong");
                response.Data = false;
                return response;
            }
          
        }
    }
}
