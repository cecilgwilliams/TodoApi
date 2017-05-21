using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRespository;

        public TodoController(ITodoRepository todoRepository)
        {
            _todoRespository = todoRepository;
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll(){
            return _todoRespository.GetAll();
        }

        [HttpGet("{id}", Name="GetTodo")]
        public IActionResult GetById(long id){
            var item = _todoRespository.Find(id);
            if (item == null){
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item){
            if (item == null){
                return BadRequest();
            }

            _todoRespository.Add(item);

            return CreatedAtRoute("GetTodo", new { id = item.Key }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item){
            if (item == null || item.Key != id){
                return BadRequest();
            }

            var todo = _todoRespository.Find(id);
            if (todo == null){
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _todoRespository.Update(todo);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id){
            var todo = _todoRespository.Find(id);
            if (todo == null){
                return NotFound();
            }

            _todoRespository.Remove(id);
            return new NoContentResult();
        }
    }
}
