using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPFilmsRaiting.Data;
using CPFilmsRaiting.Models;
using CPFilmsRaiting.Services;
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
            comment.UserName = name;
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
    }
}