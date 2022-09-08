using TimeManager.IdP.Data.Response;
using System.Security.Cryptography;
using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;

namespace TimeManager.IdP.Processors.AuthenticationProcessor
{
    public class User_IsAuth : Processor
    {
        public User_IsAuth(DataContext _context) : base(_context) {}
        public Response<bool> IsAuthorised(Token data)
        {
            Response<bool> response;
            try
            {
                var user_token = new User_Token(_context);

                if (_context.Tokens.SingleOrDefault(t => t.token == data.token, null) == null || !user_token.CheckExpirationDate(data))
                {
                    throw new Exception("User is not authenticated");
                }

                response = new Response<bool>(true);
                return response;
            } catch (Exception ex)
            {
                response = new Response<bool>(ex, "Whoops something went wrong");
                response.Data = false;
                return response;
            }
          
        }
    }
}
