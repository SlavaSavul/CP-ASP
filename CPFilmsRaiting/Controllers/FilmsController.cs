using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPFilmsRaiting.Data;
using CPFilmsRaiting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
            if (Request.Query["page"] != "" && Request.Query["limit"] != "")
            {
                int page = int.Parse(Request.Query["page"]);
                int limit = int.Parse(Request.Query["limit"]);

                return films.Skip((page-1) * limit).Take(limit);
            }
            return films;
        }

        [HttpGet("{id}")]
        public IEnumerable<FilmModel> Get(int id)
        {
            return _unitOfWork.Films.GetAll();
        }
    }
}