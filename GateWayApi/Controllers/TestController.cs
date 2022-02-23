using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GateWayApi.Models;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using Microsoft.Extensions.Configuration;

namespace GateWayApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {  
        public Producer producer { get; set; }
        private readonly IPublisher publisher;
        private readonly IConfiguration _config;
        public TestController(IPublisher publisher,IConfiguration config)
        {
            producer = new Producer();
            this.publisher = publisher;
            _config = config;
        }
        [HttpGet]
        public IActionResult Index(){
            publisher.Publish("", "todo.test", null);
            return Ok("running:" +_config.GetValue<string>("AppSettings:Key") );
        }
        [HttpPost("todo")]
        public IActionResult Create([FromBody] ToDo item){
            if(item == null)
                return BadRequest("Not null");
            Message message = new Message(){code ="00", type="post", item = item};
            publisher.Publish(JsonConvert.SerializeObject(message), "todo.create", null);
            return Ok(message);
        }
        [HttpPut("todo")]
        public IActionResult Update([FromBody] ToDo item){
            if(item == null)
                return BadRequest("Not null");
            Message message = new Message(){code ="00", type="put", item = item};
            publisher.Publish(JsonConvert.SerializeObject(message), "todo.update", null);
            return Ok(message);
        }
        [HttpDelete("todo")]
        public IActionResult Delete([FromBody] ToDo item){
            if(item == null)
                return BadRequest("Not null");
            Message message = new Message(){code ="00", type="delete", item = item};
            publisher.Publish(JsonConvert.SerializeObject(message), "todo.delete", null);
            return Ok(message);
        }
    }
}
