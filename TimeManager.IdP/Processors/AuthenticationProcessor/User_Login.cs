using TimeManager.IdP.Data.Response;
using System.Security.Cryptography;
using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.Data.Token;

namespace TimeManager.IdP.Processors.TokenProcessor
{
    public class User_Login : Processor
    {
        public User_Login(DataContext context, ILogger<TokenController> logger) : base(context, logger) { }
        public Response<TokenDTO> Login(UserDTO data)
        {
            Response<TokenDTO> response;
            try
            {
                var generateToken = new Token_Generate(_context, _logger);

                var user = _context.Users.First(u => u.UserName == data.UserName);

                if (user == null) throw new Exception($"There is no user with {data.UserName} Username");

                if (!VerifyPasswordHash(data.Password, user)) throw new Exception("Wrong Password or Username");    


                string token = generateToken.GenerateToken(user);

                user.Token = token;
                _context.SaveChanges();

                response = new Response<TokenDTO>(new TokenDTO { token = token, userId = user.Id });
                _logger.LogInformation("User successfully logged in");
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new Response<TokenDTO>(ex, "Whoops something went wrong");
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
