using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using SchoolManagementApi.Models;
using SchoolManagementApi.Services;

namespace SchoolManagementApi.Controllers {
    //[EnableCors("AllowAll")]
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

        [HttpGet]
        [Route("[controller]/{id}")]
        [Route("[controller]({id})")]
        public ActionResult GetById(int id) {
            var result = _studentService.GetById(id);
            if (result.Failure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet]
        [Route("School/{schoolId}/Students")]
        public ActionResult GetStudentsBySchoolId(int schoolId) {
            return Ok(_studentService.GetStudentsBySchoolId(schoolId));
        }

        [HttpPost]
        public ActionResult Post(Student student) {
            if (student == null) return BadRequest();

            var result = _studentService.Create(student);
            if (result.Failure) return BadRequest(result.Error);

            student = result.Value;

            return Created($"{Request.Path}/{student.Id}", student);
        }

        [HttpPut]
        [Route("[controller]/{id}")]
        [Route("[controller]({id})")]
        public ActionResult Put(Student updatedStudent, int id) {
            if (updatedStudent == null) return BadRequest();

            updatedStudent.Id = id;
            var result = _studentService.Update(updatedStudent);
            if (result.Failure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete]
        [Route("[controller]/{id}")]
        [Route("[controller]({id})")]
        public ActionResult Delete(int id) {
            var result = _studentService.RemoveById(id);
            if (result.Failure) return BadRequest(result.Error);

            return NoContent();
        }
    }
}
