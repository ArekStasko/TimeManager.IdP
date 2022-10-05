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
    public class RefreshToken_Generate : Processor
    {
        public RefreshToken_Generate(DataContext context, ILogger<TokenController> logger) : base(context, logger) { }

        public (string key, string jwt) GenerateRefreshToken(User user)
        {
            try
            {
                var randomNumber = new byte[32];

                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomNumber);
                    Convert.ToBase64String(randomNumber);
                }

                var key = System.Text.Encoding.ASCII.GetString(randomNumber);

                string jwt = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(_context.TokenKey.First().ToString())
                    .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(4).ToUnixTimeSeconds())
                    .AddClaim("refresh", randomNumber)
                    .AddClaim("username", user.UserName)
                    .Encode();

                return (key, jwt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

        }
    }
}
