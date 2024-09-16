using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using SchoolManagementApi.Models;
using SchoolManagementApi.Services;

namespace SchoolManagementApi.Controllers {
    public class StudentController : ODataController {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService) {
            _studentService = studentService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult Get() { 
            return Ok(_studentService.GetAll()); 
        }

        [HttpGet("[controller]/{id}")]
        public ActionResult GetById([FromODataUri] int id) {
            var result = _studentService.GetById(id);
            if (result.Failure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost("[controller]/create")]
        public ActionResult Post([FromBody] Student student) {
            if (student == null) return BadRequest("Student object is null");

            var result = _studentService.Create(student);
            if (result.Failure) return BadRequest(result.Error);

            student = result.Value;

            return Created($"{Request.Path}/{student.Id}", student);
        }

        [HttpPut("[controller]/update/{id}")]
        public ActionResult Put([FromODataUri] int id, [FromBody] Student updatedStudent) {
            if (updatedStudent == null) return BadRequest("Student object is null");

            updatedStudent.Id = id;
            var result = _studentService.Update(updatedStudent);
            if (result.Failure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("[controller]/delete/{id}")]
        public ActionResult Delete([FromODataUri] int id) {
            var result = _studentService.RemoveById(id);
            if (result.Failure) return BadRequest(result.Error);

            return NoContent();
        }
    }
}
