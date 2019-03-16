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
    [Route("api/[controller]")]
    [DisableCors]
    public class ValuesController : Controller
    {
        UnitOfWork _unitOfWork { get; set; }

        public ValuesController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //////////////////////////////////////////

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

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
