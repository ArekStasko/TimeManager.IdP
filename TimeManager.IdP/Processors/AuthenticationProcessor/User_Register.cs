using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data;
using System.Security.Cryptography;
using TimeManager.IdP.Authentication;


namespace TimeManager.IdP.Processors.AuthenticationProcessor
{
    public class User_Register : Processor
    {
        public User_Register(DataContext context, ILogger<TokenController> logger) : base(context, logger) { }
        public Response<Token> Register(UserDTO data)
        {
            Response<Token> response;
            try
            {
                _logger.LogInformation("Register Processor invoked");
                Tuple<byte[], byte[]> hash = CreatePasswordHash(data.Password);

                if(_context.Users.Any(u => u.UserName == data.UserName))
                {
                    throw new Exception("User with this username already exists");
                }
                _logger.LogInformation($"Username: {data.UserName}  Is free");

                User user = new User(data.UserName, hash.Item1, hash.Item2);
                _context.Users.Add(user);
                _context.SaveChanges();

                var user_token = new Token_Generate(_context, _logger);
                _logger.LogInformation("Token is created");

                Token token = user_token.CreateToken(user);
                response = new Response<Token>(token);

                _logger.LogInformation("Successfully registered user");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new Response<Token>(ex, "Whoops something went wrong");
                return response;
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
                throw new Exception(ex.Message);
            }
        }
    }
}
