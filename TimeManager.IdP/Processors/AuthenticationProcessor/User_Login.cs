using TimeManager.IdP.Data.Response;
using System.Security.Cryptography;
using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;

namespace TimeManager.IdP.Processors.AuthenticationProcessor
{
    public class User_Login : Processor
    {
        public User_Login(DataContext context) : base(context) { }
        public Response<Token> Login(UserDTO data)
        {
            Response<Token> response;
            try
            {
                var user_token = new User_Token(_context);
                var user = _context.Users.FirstOrDefault(u => u.UserName == data.UserName);

                if (VerifyPasswordHash(data.Password, user))
                {

                    Token token = user_token.CreateToken(user);
                    response = new Response<Token>(token);
                    return response;
                }

                throw new Exception("User not found !");
            }
            catch (Exception ex)
            {
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
