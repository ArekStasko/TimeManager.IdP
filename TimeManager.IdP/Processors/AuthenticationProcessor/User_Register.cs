using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data;
using System.Security.Cryptography;
using TimeManager.IdP.Authentication;


namespace TimeManager.IdP.Processors.AuthenticationProcessor
{
    public class User_Register : Processor
    {
        public User_Register(DataContext context) : base(context) { }
        public Response<Token> Register(UserDTO data)
        {
            Response<Token> response;
            try
            {
                Tuple<byte[], byte[]> hash = CreatePasswordHash(data.Password);

                User user = new User(data.UserName, hash.Item1, hash.Item2);
                _context.Users.Add(user);
                _context.SaveChanges();

                var user_token = new User_Token(_context);

                Token token = user_token.CreateToken(user);
                response = new Response<Token>(token);

                return response;
            }
            catch (Exception ex)
            {
                response = new Response<Token>(ex, "Whoops something went wrong");
                return response;
            }
        }

        private Tuple<byte[], byte[]> CreatePasswordHash(string password)
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
    }
}
