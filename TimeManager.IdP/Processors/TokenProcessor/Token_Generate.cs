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

namespace TimeManager.IdP.Processors.AuthenticationProcessor
{
    public class Token_Generate : Processor
    {
        public Token_Generate(DataContext context, ILogger<TokenController> logger) : base(context, logger) { }

        public string GenerateToken(User user)
        {
            try
            {
                string token = new JwtBuilder()
                     .WithAlgorithm(new HMACSHA256Algorithm())
                     .WithSecret(Encoding.ASCII.GetBytes(_context.TokenKey.First().ToString()))
                     .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds())
                     .AddClaim("username", user.UserName)
                     .Encode();
                    
                return token;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            
        }                                  

    }
}
