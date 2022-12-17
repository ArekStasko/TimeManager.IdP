using TimeManager.IdP.Data.Response;
using System.Security.Cryptography;
using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.Processors.TokenProcessor;
using TimeManager.IdP.Data.Token;

namespace TimeManager.IdP.Processors.UserProcessor
{
    public class User_Login : Processor, IUser_Login
    {
        public User_Login(DataContext context, ILogger<TokenController> logger) : base(context, logger) { }
        public TokenDTO Execute(UserDTO data)
        {
            try
            {
                var generateToken = new Token_Generate(_context, _logger);

                var user = _context.Users.First(u => u.UserName == data.UserName);

                if (user == null) throw new Exception($"There is no user with {data.UserName} Username");

                if (!VerifyPasswordHash(data.Password, user)) throw new Exception("Wrong Password or Username");    


                string token = generateToken.Execute(user);

                user.Token = token;
                _context.SaveChanges();

                _logger.LogInformation("User successfully logged in");
                return new TokenDTO { token = token, userId = user.Id };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
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
