using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoApi.Controllers
{
    [ApiController]
    //[Produces("application/json")]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        public ToDoContext _context { get; set; }
        public ToDoController(ToDoContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var db = await _context.ToDos.ToArrayAsync();
            return Ok(db);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id){
            var item = await _context.ToDos.Where(p => p.Id == id).FirstOrDefaultAsync();
            if(item == null)
                return Ok("Not found");
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToDo item){
            if(_context.ToDos.Any(p => p.Id == item.Id)){
                return BadRequest("Item has exist");
            }
            await _context.ToDos.AddAsync(item);
            await _context.SaveChangesAsync();
            return Ok(item);
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] ToDo item){
            ToDo todo = _context.ToDos.Where(p => p.Id == id).FirstOrDefault();
            if (todo != null)
            {
                _context.Entry(todo).CurrentValues.SetValues(item);
            }
            else{
                return BadRequest("Item not exist");
            }
            await _context.SaveChangesAsync();
            return Ok(item);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id){
            ToDo todo = _context.ToDos.Where(p => p.Id == id).FirstOrDefault();
            if (todo != null)
            {
                _context.ToDos.Remove(todo);
                await _context.SaveChangesAsync();
                return Ok(todo);
            }
            else{
                return BadRequest("Item not exist");
            }
            
        }

    }
}
