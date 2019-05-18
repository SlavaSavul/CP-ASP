using System;
using System.Collections.Generic;
using System.Globalization;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CPFilmsRaiting.Controllers
{
    [Route("api/films")]
    public class FilmsController : Controller
    {
        DbService _dbService { get; set; }

        public FilmsController(DbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public void Get()
        {
            int page = -1;
            int limit = -1;
            int year = -1;
            page = int.Parse(Request.Query["page"]);
            limit = int.Parse(Request.Query["limit"]);
            int.TryParse(Request.Query["year"].ToString(), out year);
            string raitingAsString = Request.Query["raiting"];
            double raiting = -1;
            bool favorite = false;
            string name = Request.Query["name"].ToString();
            string genres2 = Request.Query["genres"].ToString();

            if (raitingAsString != null)
            {
                double.TryParse(raitingAsString.Replace(".", ","), out raiting);
            }
            
            favorite = !StringValues.IsNullOrEmpty(Request.Query["favorite"]) ? bool.Parse(Request.Query["favorite"]) : false;

            var response = _dbService.Get(page, limit, year, raiting, name, genres2, favorite, GetUserId());

            if (page < 1 || limit < 1 || response == null)
            {
                WriteResponseError("Not found", 400);
            }
            else
            {
                WriteResponseData(response);
            }
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _dbService.Delete(id);
        }

        [HttpGet]
        [Route("genres")]
        public List<string> GetGanres()
        {
            return _dbService.GetGanres();
        }

        [HttpGet("{id}")]
        public void Get(string id)
        {
            var response = new
            { 
                data = _dbService.Get(id) 
            };
            WriteResponseData(response);
        }

        [HttpGet("{id}/comments")]
        public void GetComments(string id)
        {
            var response = new
            {
                comments = _dbService.GetComments(id)
            };
            WriteResponseData(response);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public void Post([FromBody]FilmModel film)
        {
            _dbService.Create(film);

            if (film.Id != null) {
                var response = new
                {
                    film
                };
                WriteResponseData(response);
            }
            else
            {
                WriteResponseError("Already exists", 400);
            }
        }

        [HttpPost("like")]
        [Authorize]
        public void Post([FromBody] LikeModel likeModel)
        {
            var userId = GetUserId();
            _dbService.CreateLike(userId, likeModel);
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

        [HttpGet("like")]
        [Authorize]
        public void GetLikes()
        {
            var userId = GetUserId();

            if (userId != null)
            {
                var response = new
                {
                    likes = _dbService.GetUserLikes(userId)
                };
                WriteResponseData(response);
            }
            else
            {
                var response = new { };
                WriteResponseData(response);
            }
        }



        [HttpPut]
        [Authorize(Roles = "admin")]
        public void Put([FromBody]FilmModel film)
        {
            _dbService.Update(film);
            if (film.Id != null)
            {
                var response = new
                {
                    data = film
                };
                WriteResponseData(response);
            }
            else
            {
                WriteResponseError("Error", 400);
            }
        }

        private void WriteResponseError(string message, int statusCode)
        {
            Response.StatusCode = statusCode;
            Response.ContentType = "application/json";
            var response = new
            {            
                message
            };

            Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private void WriteResponseData(object response)
        {
            Response.ContentType = "application/json";
           
            var serializerSettings = new JsonSerializerSettings();

            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            serializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
        }

    }
}