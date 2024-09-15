using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using SchoolManagementApi.Models;
using SchoolManagementApi.Services;

namespace SchoolManagementApi.Controllers {
    //[EnableCors("AllowAll")]
    public class SchoolController : ODataController {
        private readonly SchoolService _schoolService;

        public SchoolController(SchoolService schoolService) {
            _schoolService = schoolService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult Get() {
            return Ok(_schoolService.GetAll());
        }

        [HttpGet]
        [Route("[controller]/{id}")]
        [Route("[controller]({id})")]
        public ActionResult GetById(int id) {
            var result = _schoolService.GetById(id);
            if (result.Failure) return BadRequest(result.Error);

            return Ok(result.Value); 
        }

        [HttpPost]
        public ActionResult Post(School school) {
            if (school == null) return BadRequest();

            var result = _schoolService.Create(school);
            if(result.Failure) return BadRequest(result.Error);

            school = result.Value;

            return Created($"{Request.Path}/{school.Id}", school);
        }

        [HttpPut]
        [Route("[controller]/{id}")]
        [Route("[controller]({id})")]
        public ActionResult Put(School updatedSchool, int id) {
            if (updatedSchool == null) return BadRequest();

            updatedSchool.Id = id;
            var result = _schoolService.Update(updatedSchool);
            if (result.Failure) return BadRequest(result.Error);

            return Ok(result.Value); 
        }

        [HttpDelete]
        [Route("[controller]/{id}")]
        [Route("[controller]({id})")]
        public ActionResult Delete(int id) {
            var result = _schoolService.RemoveById(id);
            if (result.Failure) return BadRequest(result.Error);

            return NoContent();
        }
    }
}
