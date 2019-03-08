using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPFilmsRaiting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CPFilmsRaiting.Controllers
{
    [Route("api/[controller]")]
    [DisableCors]
    public class ValuesController : Controller
    {
        [Authorize]
        [Route("getlogin")]
        public JsonResult GetLogin()
        {
            ApplicationUser a = new ApplicationUser();
            a.Login = "asd";
            return Json(a);
        }

        [Authorize(Roles = "admin")]
        [Route("getrole")]
        public IActionResult GetRole()
        {
            return Ok("{a: 'Ваша роль: администратор'}");
        }

        ///////////////////////////////////////////////

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
