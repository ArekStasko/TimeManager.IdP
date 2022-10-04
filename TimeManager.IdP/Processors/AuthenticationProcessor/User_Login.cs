using TimeManager.IdP.Data.Response;
using System.Security.Cryptography;
using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;

namespace TimeManager.IdP.Processors.AuthenticationProcessor
{
    public class User_Login : Processor
    {
        public User_Login(DataContext context, ILogger<TokenController> logger) : base(context, logger) { }
        public Response<Token> Login(UserDTO data)
        {
            Response<Token> response;
            try
            {
                var user_token = new Token_Generate(_context, _logger);
                var user = _context.Users.FirstOrDefault(u => u.UserName == data.UserName);
                Token token = null;

                if (!VerifyPasswordHash(data.Password, user))
                {
                    throw new Exception("Wrong Password or Username");    
                }

                if (_context.Tokens.SingleOrDefault(u => u.Id == user.Id, null) != null)
                {
                    token = _context.Tokens.Single(u => u.Id == user.Id);
                }

                if (user_token.CheckExpirationDate(token))
                {
                    response = new Response<Token>(token);
                    return response;
                }

                token = user_token.CreateToken(user);
                response = new Response<Token>(token);
                _logger.LogInformation("User successfully logged in");
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new Response<Token>(ex, "Whoops something went wrong");
                return response;
            }
        }

        private bool VerifyPasswordHash(string password, User user)
        {
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }
    }
}
