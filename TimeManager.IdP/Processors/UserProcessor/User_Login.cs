using System.Security.Cryptography;
using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.Processors.TokenProcessor;
using TimeManager.IdP.Data.Token;
using LanguageExt.Common;

namespace TimeManager.IdP.Processors.UserProcessor
{
    public class User_Login : Processor<AuthController>, IUser_Login
    {
        public User_Login(DataContext context, ILogger<AuthController> logger) : base(context, logger) { }
        public async Task<Result<TokenDTO>> Execute(UserDTO data)
        {
            try
            {
                var generateToken = new Token_Generate(_context, _logger);

                var user = _context.Users.First(u => u.UserName == data.UserName);

                if (user == null) return new Result<TokenDTO>(new Exception($"There is no user with {data.UserName} Username"));

                if (!VerifyPasswordHash(data.Password, user)) return new Result<TokenDTO>(new Exception("Wrong Password or Username"));    


                string token = generateToken.Execute(user);

                user.Token = token;
                _context.SaveChanges();

                _logger.LogInformation("User successfully logged in");
                var tokenDTO = new TokenDTO { token = token, userId = user.Id };
                return new Result<TokenDTO>(tokenDTO);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<TokenDTO>(ex);
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
