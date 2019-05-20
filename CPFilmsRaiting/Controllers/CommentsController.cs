using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CPFilmsRaiting.Data;
using CPFilmsRaiting.Models;
using CPFilmsRaiting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CPFilmsRaiting.Controllers
{
    public class CommentsController : Controller
    {
        DbService _dbService { get; set; }

        public CommentsController(DbService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [Route("api/comments")]
        public void Post([FromBody]CommentModel comment)
        {
           string name = User.Identity.Name;
            comment.Date = DateTime.Now;
            comment.UserId = GetUserId();
            _dbService.CreateComment(comment);
        }

        private void WriteResponseData(object response)
        {
            Response.ContentType = "application/json";
            var serializerSettings = new JsonSerializerSettings();

            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            serializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
        }

        private string GetUserId()
        {
            StringValues authorizationHeader;
            Request.Headers.TryGetValue("Authorization", out authorizationHeader);
            string token = authorizationHeader.ToString().Split(" ")[1];

            if (token != "null")
            {
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims;
                var jti = claims.First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value;
                var userId = _dbService.GetUser(jti).Id;
                return userId;
            }
            else
            {
                return null;
            }
        }
    }
}