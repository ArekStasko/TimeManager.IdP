using System.Security.Cryptography;
using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace TimeManager.IdP.Processors.AuthenticationProcessor
{
    public class User_Token : Processor
    {
        public User_Token(DataContext context) : base(context) { }

        public Token CreateToken(User user)
        {
            var token = _context.Tokens.SingleOrDefault(t => t.userId == user.Id);   
            if (token != null && CheckExpirationDate(token))
            {
                return token;
            }


            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_context.TokenKey.FirstOrDefault().Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            token = new Token(jwt);
            token.userId = user.Id;
            token.createDate = DateTime.Now;
            token.expirationDate = DateTime.Now.AddDays(1);
            _context.Tokens.Add(token);
            _context.SaveChanges();
            return token;
        }

        public bool CheckExpirationDate(Token token)
        {
            int tokenTime = DateTime.Now.CompareTo(token.expirationDate);
            if (tokenTime > 0) _context.Tokens.Remove(token);
            return tokenTime < 0;
        }


    }
}
