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
        public IEnumerable<FilmModel> Get()
        {
            IEnumerable<FilmModel> films = _unitOfWork.Films.GetAll();
            if (
                !StringValues.IsNullOrEmpty(Request.Query["page"]) && 
                !StringValues.IsNullOrEmpty(Request.Query["limit"])
            )
            {
                int page = int.Parse(Request.Query["page"]);
                int limit = int.Parse(Request.Query["limit"]);

                return films.Skip((page-1) * limit).Take(limit);
            }
            return films;
        }

        [HttpGet("{id}")]
        public FilmModel Get(string id)
        {
            return _unitOfWork.Films.Get(id);
        }

        [HttpPost]
        public void Post([FromBody]FilmModel film)
        {
            _unitOfWork.Films.Create(film);
            if (film.Id != null) {
                WriteResponseData(film);
            }
            else
            {
                WriteResponseError("Already exists", 400);
            }
        }

        [HttpPut]
        public void Put([FromBody]FilmModel film)
        {
            _unitOfWork.Films.Update(film);
            if (film.Id != null)
            {
                WriteResponseData(film);
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

        private void WriteResponseData(FilmModel film)
        {
            Response.ContentType = "application/json";
            var response = new
            {
                data = film
            };
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
        }

    }
}