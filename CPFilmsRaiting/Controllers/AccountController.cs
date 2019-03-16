using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CPFilmsRaiting.Data;
using CPFilmsRaiting.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CPFilmsRaiting.Controllers
{
    [DisableCors]
    public class AccountController : Controller
    {
        UnitOfWork _unitOfWork;
        public AccountController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("/registration")]
        public async Task Registration([FromBody]ApplicationUser user)
        {
            ApplicationUser newUser = new ApplicationUser() {
                Email = user.Email,
                Password = user.Password,
                Role = "user"
            };
            if (!isValid(newUser))
            {
               await WriteResponseError("Invalid model validation", 400);
            }
            else if (_unitOfWork.Users.GetByEmail(newUser.Email) != null)
            {
                await WriteResponseError("Such email exists", 400);
            }
            else
            {
                _unitOfWork.Users.Create(newUser);

                await WriteResponseData(newUser);
            }
        }

        private bool isValid(ApplicationUser user)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(user);
            if (!Validator.TryValidateObject(user, context, results, true))
            {
                return false;
            }
            return true;
        }

        [HttpPost("/login")]
        public async Task Login([FromBody]ApplicationUser user)
        {
            if (_unitOfWork.Users.IsExists(user.Email, user.Password))
            {
                await WriteResponseData(user);
            }
            else
            {
                await WriteResponseError("Invalid email or password", 400);
            }
        }

        private async Task WriteResponseError(string message, int status)
        {
            var response = new
            {
                message
            };
            Response.StatusCode = status;
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(
                    response,
                    new JsonSerializerSettings { Formatting = Formatting.Indented }
            ));
        }

        private async Task WriteResponseData(ApplicationUser user)
        {
            var identity = GetIdentity(user.Email, user.Password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password");
            }
            else 
            {
                var encodedJwt = CreateJWTToken(user, identity);
                var response = new
                {
                    data = new
                    {
                        access_token = encodedJwt,
                        username = identity.Name,
                        role = _unitOfWork.Users.GetByEmail(user.Email).Role
                    }
                };

                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonConvert.SerializeObject(
                    response,
                    new JsonSerializerSettings { Formatting = Formatting.Indented }
                ));
            }
        }

        private string CreateJWTToken(ApplicationUser user, ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(
                        AuthOptions.GetSymmetricSecurityKey(), 
                        SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            ApplicationUser user = _unitOfWork.Users.GetByEmail(username);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}