using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        public StudentController(Database db)
        {
            Db = db;
        }

        // GET api/Student
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new Student(Db);
            var result = await query.GetAllAsync();
            return new OkObjectResult(result);
        }

        // GET api/Student/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new Student(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Student
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Student body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/Student/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]Student body)
        {
            await Db.Connection.OpenAsync();
            var query = new Student(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.fname = body.fname;
            result.lname = body.lname;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new Student(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkObjectResult(result);
        }

        public Database Db { get; }
    }
}