using System;
using System.Collections.Generic;
using System.Linq;
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
            IEnumerable<FilmModel> films = _unitOfWork.Films.GetAll();
            IEnumerable<FilmModel> result = films.ToList();
            count = films.Count();

            if (
                !StringValues.IsNullOrEmpty(Request.Query["page"]) && 
                !StringValues.IsNullOrEmpty(Request.Query["limit"])
            )
            {
                page = int.Parse(Request.Query["page"]);
                limit = int.Parse(Request.Query["limit"]);

                result = films.Skip((page-1) * limit).Take(limit);
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

        [HttpGet("{id}")]
        public FilmModel Get(string id)
        {
            return _unitOfWork.Films.Get(id);
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
            Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
        }

    }
}