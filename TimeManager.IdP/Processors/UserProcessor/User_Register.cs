using TimeManager.IdP.Data;
using System.Security.Cryptography;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.Data.Token;
using TimeManager.IdP.Processors.TokenProcessor;
using LanguageExt.Common;
using TimeManager.IdP.services.MessageQueuer;

namespace TimeManager.IdP.Processors.UserProcessor
{
    public class User_Register : Processor<AuthController>, IUser_Register
    {
        public User_Register(DataContext context, ILogger<AuthController> logger, IMQManager mqManager) : base(context, logger, mqManager) { }
        public async Task<Result<TokenDTO>> Execute(UserDTO data)
        {
            try
            {
                _logger.LogInformation("Register Processor invoked");
                Tuple<byte[], byte[]> hash = CreatePasswordHash(data.Password);

                if(_context.Users.Any(u => u.UserName == data.UserName))
                {
                    return new Result<TokenDTO>(new Exception("User with this username already exists"));
                }

                User user = new User(data.UserName, hash.Item1, hash.Item2);
                _context.Users.Add(user);
                

                var generateToken = new Token_Generate(_context, _logger);

                string token = generateToken.Execute(user);
                _logger.LogInformation("Token is created");

                user.Token = token;
                _context.SaveChanges();

                user = _context.Users.Single(u => u.UserName == user.UserName);
                bool succ = _mqManager.Publish(
                    user,
                    "entity.user.post",
                    "direct",
                    "user_Post"
                );
                _logger.LogInformation("Received message");
                if (!succ)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    return new Result<TokenDTO>(new Exception("Some Processing Engine Error was thrown"));
                }
                


                _logger.LogInformation("Successfully registered user");
                var tokenDTO = new TokenDTO() { token = token, userId = user.Id };
                return new Result<TokenDTO>(tokenDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<TokenDTO>(ex);
            }
        }

        private Tuple<byte[], byte[]> CreatePasswordHash(string password)
        {
            try
            {
                byte[] passwordSalt;
                byte[] passwordHash;

                using (var hmac = new HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }

                return new Tuple<byte[], byte[]>(passwordHash, passwordSalt);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
