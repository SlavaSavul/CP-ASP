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
using CPFilmsRaiting.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CPFilmsRaiting.Controllers
{
    [DisableCors]
    public class AccountController : Controller
    {
        DbService _dbService { get; set; }
        private readonly IHttpContextAccessor _context;

        public AccountController(DbService dbService, IHttpContextAccessor context)
        {
            _context = context;
            _dbService = dbService;
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
            else if (_dbService.GetUser(newUser.Email) != null)
            {
                await WriteResponseError("Such email exists", 400);
            }
            else
            {
                _dbService.CreateUser(newUser);
                await WriteResponseData(newUser);
            }
        }

        [HttpPost("/authentication")]
        public async Task Authentication()
        {
            JwtSecurityToken jwtToken = GetJwtSecurityToken();
            if (jwtToken != null && jwtToken.ValidTo < DateTime.UtcNow)
            {
                string jwt = CreateJWTToken(jwtToken.Claims);

                var response = new
                {
                    data = new
                    {
                        access_token = jwt
                    }
                };

                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonConvert.SerializeObject(
                    response,
                    new JsonSerializerSettings { Formatting = Formatting.Indented }
                ));
            }
            else
            {
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonConvert.SerializeObject(
                    new {
                        data = new {}
                    },
                    new JsonSerializerSettings { Formatting = Formatting.Indented }
                ));
            }
        }

        public JwtSecurityToken GetJwtSecurityToken()
        {
            
            StringValues authorizationHeader;
            Request.Headers.TryGetValue("Authorization", out authorizationHeader);
            string token = authorizationHeader.ToString().Split(" ")[1];

            if (token != "null")
            {
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = handler.ReadJwtToken(token);
                return jwtToken;
            }
            else
            {
                return null;
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
            if (_dbService.IsUserExists(user.Email, user.Password))
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
                var encodedJwt = CreateJWTToken(identity.Claims);
                var response = new
                {
                    data = new
                    {
                        access_token = encodedJwt,
                        username = identity.Name,
                        role = _dbService.GetUser(user.Email).Role
                    }
                };

                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonConvert.SerializeObject(
                    response,
                    new JsonSerializerSettings { Formatting = Formatting.Indented }
                ));
            }
        }

        private string CreateJWTToken(IEnumerable<Claim> Claims)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(
                        AuthOptions.GetSymmetricSecurityKey(), 
                        SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            ApplicationUser user = _dbService.GetUser(username);
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