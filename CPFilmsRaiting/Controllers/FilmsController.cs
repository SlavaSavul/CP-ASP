﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CPFilmsRaiting.Data;
using CPFilmsRaiting.Models;
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
    [DisableCors]
    public class FilmsController : Controller
    {
        UnitOfWork _unitOfWork { get; set; }

        public FilmsController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public void Get()
        {
            int page = -1;
            int limit = -1;
            int count = 0;
            IEnumerable<FilmModel> films = _unitOfWork.Films.GetAllWithInclude();
            IEnumerable<FilmModel> result = films.ToList();
            count = films.Count();

            bool favorite = false;
            if (!StringValues.IsNullOrEmpty(Request.Query["favorite"]))
            {
                var favoriteFilms = new List<FilmModel>();
                favorite = bool.Parse(Request.Query["favorite"]);
                var likes = _unitOfWork.Likes.GetByUserId(GetUserId());
                foreach(LikeModel like in likes)
                {
                    favoriteFilms.Add(_unitOfWork.Films.GetWithoutInclude(like.FilmId));
                }
                result = favoriteFilms;
            }

            int year = 0;
            if (int.TryParse(Request.Query["year"].ToString(), out year) )
            {
                result = result.Where( f => f.Date.Year == year);
            }

            string raitingParam = Request.Query["raiting"];
            double raiting = 0;
            if (raitingParam != null && double.TryParse(raitingParam.Replace(".",","), out raiting))
            {
                result = result.Where(f => ((double)f.IMDbRaiting) >= raiting);
            }

            if (Request.Query["name"].ToString() != "")
            {
                result = result.Where(f => f.Name.Contains(Request.Query["name"]));
            }

            if (Request.Query["genres"].ToString() != "")
            {
                List<string> genres = Request.Query["genres"].ToList();
                result = result.Where(film => {
                    List<string> filmGenres = film.Genres.Select(g => g.Genre).ToList();
                    List<string> exists = filmGenres.Where(g => genres.Any(g2 => g.Equals(g2))).ToList();
                    return exists.Count() > 0;
                });
            }

            count = result.Count();
            if (
               !StringValues.IsNullOrEmpty(Request.Query["page"]) &&
               !StringValues.IsNullOrEmpty(Request.Query["limit"])
           )
            {
                page = int.Parse(Request.Query["page"]);
                limit = int.Parse(Request.Query["limit"]);
                result = result.Skip((page - 1) * limit).Take(limit);
            }            

            if (page < 1 || limit < 1 || result.Count() < 1)
            {
                WriteResponseError("Not found", 400);
            }
            else
            {
                var response = new
                {
                    films = result,
                    metaData = new
                    {
                        page,
                        limit,
                        count
                    }
                };

                WriteResponseData(response);
            }
        }
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _unitOfWork.Films.Delete(id);
        }

        [HttpGet]
        [Route("genres")]
        public List<string> GetGanres()
        {
            List<GenreModel> a = _unitOfWork.Films.GetAllWithInclude().SelectMany(f => f.Genres).ToList();

            return a.Select(g => g.Genre).Distinct().ToList();
        }

        [HttpGet("{id}")]
        public void Get(string id)
        {
            var response = new
            {
                data = _unitOfWork.Films.Get(id)
            };
            WriteResponseData(response);
        }

        [HttpGet("{id}/comments")]
        public void GetComments(string id)
        {
            var response = new
            {
                comments = _unitOfWork.Comments.GetAll(id)
            };
            WriteResponseData(response);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public void Post([FromBody]FilmModel film)
        {
            _unitOfWork.Films.Create(film);
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
            _unitOfWork.Likes.Create(new LikeModel() {UserId = userId, FilmId = likeModel.FilmId});
          
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
                var userId = _unitOfWork.Users.GetByEmail(jti).Id;
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
            StringValues authorizationHeader;
            Request.Headers.TryGetValue("Authorization", out authorizationHeader);
            string token = authorizationHeader.ToString().Split(" ")[1];

            if (token != "null")
            {
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims;
                var jti = claims.First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value;
                var userId = _unitOfWork.Users.GetByEmail(jti).Id;
                var response = new
                {
                    likes = _unitOfWork.Likes.GetByUserId(userId)
                };
                WriteResponseData(response);
            }
            else
            {
                var response = new
                {
                };
                WriteResponseData(response);
            }
        }



        [HttpPut]
        [Authorize(Roles = "admin")]
        public void Put([FromBody]FilmModel film)
        {
            _unitOfWork.Films.Update(film);
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