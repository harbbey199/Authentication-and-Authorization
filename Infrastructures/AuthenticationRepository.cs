using Contracts;
using Credential_Auth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures
{
    public class AuthenticationRepository:IAuthentication
    {
        private readonly IConfiguration _configuration;
        private readonly AppContexts _appContext;

        public AuthenticationRepository(IConfiguration configuration, AppContexts appContext)
        {
            _configuration = configuration;
            _appContext = appContext;
        }



        // Login Method which calls the Create Method after login
        public async Task<string> Login(LoginModel user)
        {
            try
            {
                var currentuser = await _appContext.users.FirstOrDefaultAsync(x => x.Username == user.UserName);
                if (currentuser is null)
                {
                    return ("Invalid Credentials");
                }

                if (user.UserName == currentuser.Username && user.Password == currentuser.Password)
                {
                    var tokenString = CreateToken(currentuser);
                    return ($"you are signed in: {tokenString}");
                }
                return "invalid credentials";

            }
            catch (Exception)
            {
                return "Internal Server Error";
            }
         

        }



        // Helper Method for creation of Tokens
        public string CreateToken(UserModel user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

            JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:ValidIssuer"],
            audience: _configuration["JwtSettings:ValidAudience"],
            claims: new List<Claim>(),
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // Register Method to register as eligible user.
        public async Task<string>Register(RegisterDto registerDto)
        {
            try
            {
               var user = await _appContext.users.FindAsync(registerDto.Email);
                if(user != null)
                {
                    return ("user already exist");
                }
                var newUser = new UserModel();
                newUser.Username = registerDto.Username;
                newUser.Password = registerDto.Password;   
                newUser.Email = registerDto.Email;  
                newUser.Id = Guid.NewGuid().ToString().Substring(10);
                _appContext.Add(newUser);
                _appContext.SaveChanges();
                return ("user registered successfully");
            }
            catch(Exception)
            {
                return ("internal server error");
            }
        }
        
    }
}
