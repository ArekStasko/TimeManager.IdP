using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TimeManager.IdP.Processors.TokenProcessor
{
    public class Token_Verify : Processor, IToken_Verify
    {
        public Token_Verify(DataContext context, ILogger<TokenController> logger) : base(context, logger) { }

 
        public bool Execute(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_context.TokenKey.First().ToString())),
                }, out _);

                var tokenResult = handler.ReadJwtToken(token);
  
                var claims = tokenResult.Claims.ToList().Where(claim => claim.Type == "Username").First();

                var user = _context.Users.Where(u => u.UserName == claims.Value);

                if (!user.Any())
                {
                    _logger.LogError("Verify Token failed");
                    throw new Exception("There is no user for this token");
                }

                return true;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            
        }                                                     
    }
}
