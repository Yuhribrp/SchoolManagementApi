using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using SchoolManagementApi.Models;
using SchoolManagementApi.Services;

namespace SchoolManagementApi.Controllers {
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

        [HttpGet("[controller]/{id}")]
        public ActionResult GetById([FromODataUri] int id) {
            var result = _schoolService.GetById(id);
            if (result.Failure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost("[controller]/create")]
        public ActionResult Post([FromBody] School school) {
            if (school == null) return BadRequest("School object is null");

            var result = _schoolService.Create(school);
            if (result.Failure) return BadRequest(result.Error);

            school = result.Value;

            return Created($"{Request.Path}/{school.Id}", school);
        }

        [HttpPut("[controller]/update/{id}")]
        public ActionResult Put([FromODataUri] int id, [FromBody] School updatedSchool) {
            if (updatedSchool == null) return BadRequest("School object is null");

            updatedSchool.Id = id;
            var result = _schoolService.Update(updatedSchool);
            if (result.Failure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("[controller]/delete/{id}")]
        public ActionResult Delete([FromODataUri] int id) {
            var result = _schoolService.RemoveById(id);
            if (result.Failure) return BadRequest(result.Error);

            return NoContent();
        }
    }
}
