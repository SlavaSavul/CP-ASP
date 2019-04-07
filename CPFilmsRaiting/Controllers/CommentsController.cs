using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPFilmsRaiting.Data;
using CPFilmsRaiting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CPFilmsRaiting.Controllers
{
    [DisableCors]
    public class CommentsController : Controller
    {

        UnitOfWork _unitOfWork { get; set; }

        public CommentsController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            comment.UserName = name;
            _unitOfWork.Comments.Create(comment);
        }

        [HttpGet("{id}")]
        [Route("api/films/{id}/comments")]
        public void GetComments(string id)
        {
            var response = new
            {
                comments = _unitOfWork.Comments.GetAll(id)
            };
            WriteResponseData(response);
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