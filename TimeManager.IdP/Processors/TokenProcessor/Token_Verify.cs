using System.Security.Cryptography;
using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using JWT.Builder;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using System.Text;

namespace TimeManager.IdP.Processors.TokenProcessor
{
    public class Token_Verify : Processor
    {
        public Token_Verify(DataContext context, ILogger<TokenController> logger) : base(context, logger) { }

        public int VerifyToken(string token)
        {
            try
            {
                var result = new JwtBuilder()
                    .WithSecret(_context.TokenKey.First().ToString())
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(token);

                var user = _context.Users.Where(u => u.UserName == result["username"]);

                if (!user.Any())
                {
                    _logger.LogError("Verify Token failed");
                    throw new Exception("There is no user");
                }

                return user.First().Id;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            
        }                                                     

    }
}
