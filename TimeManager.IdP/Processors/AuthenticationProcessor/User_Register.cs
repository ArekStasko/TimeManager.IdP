﻿using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data;
using System.Security.Cryptography;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.Data.Token;

namespace TimeManager.IdP.Processors.TokenProcessor
{
    public class User_Register : Processor
    {
        public User_Register(DataContext context, ILogger<TokenController> logger) : base(context, logger) { }
        public Response<TokenDTO> Register(UserDTO data)
        {
            Response<TokenDTO> response;
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
                

                var generateToken = new Token_Generate(_context, _logger);

                string token = generateToken.GenerateToken(user);
                _logger.LogInformation("Token is created");

                user.Token = token;
                _context.SaveChanges();

                response = new Response<TokenDTO>(new TokenDTO() { token = token, userId = user.Id });

                _logger.LogInformation("Successfully registered user");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new Response<TokenDTO>(ex, "Whoops something went wrong");
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
