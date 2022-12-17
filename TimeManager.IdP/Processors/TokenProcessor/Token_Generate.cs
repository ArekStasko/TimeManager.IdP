using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TimeManager.IdP.Processors.TokenProcessor
{
    public class Token_Generate : Processor, IToken_Generate
    {
        public Token_Generate(DataContext context, ILogger<TokenController> logger) : base(context, logger) { }

        public string Execute(User user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_context.TokenKey.First().ToString()));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("Username", user.UserName),
                };

                var token = new JwtSecurityToken(
                    null, 
                    null, 
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: credentials
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            
        }                                  

    }
}
