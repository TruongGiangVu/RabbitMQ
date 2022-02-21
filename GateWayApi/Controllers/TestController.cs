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
        [HttpGet]
        public IActionResult Index(){
            return Ok("running");
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] ToDo item){
            if(item == null)
                return BadRequest("Not null");
            producer.InitQueue("todo");
            Message message = new Message(){code ="00", type="post", item = item};
            producer.Publish(JsonConvert.SerializeObject(message));
            return Ok(item);
        }
    }
}
