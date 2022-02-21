using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GateWayApi.Models;
using Newtonsoft.Json;

namespace GateWayApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {  
        public Producer producer { get; set; }
        public TestController()
        {
            producer = new Producer();
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] ToDo item){
            if(item == null)
                return BadRequest("Not null");
            producer.InitQueue("todo");
            producer.Publish(JsonConvert.SerializeObject(item));
            return Ok(item);
        }
    }
}
