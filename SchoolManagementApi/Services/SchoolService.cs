using SchoolManagementApi.Contexts;
using SchoolManagementApi.Models;
using SchoolManagementApi.Utils;

namespace SchoolManagementApi.Services {
    public class SchoolService {

        private readonly DataBaseContext _context;

        public SchoolService(DataBaseContext context) {
            _context = context;
        }

        public Result<School> Create(School school) {
            var validationResult = ValidateSchool(school);
            if (validationResult.Failure) {
                return Result.Fail<School>(validationResult.Error);
            }

            try {

                school.Identifier = Guid.NewGuid();
                school = _context.Schools.Add(school).Entity;
                _context.SaveChanges();
            }
            catch (Exception ex) {

                return Result.Fail<School>(ex.Message);
            }

            return Result.Ok(school); 
        }

        public Result<School> GetById(int id) {
            var school = _context.Schools.Find(id);
            if (school == null) {
                return Result.Fail<School>("School Not Found!");
            }

            return Result.Ok(school);
        }

        public IQueryable<School> GetAll() => _context.Schools;

        public Result<School> Update(School updatedSchool) {
            var school = _context.Schools.Find(updatedSchool.Id);
            if (school == null) {
                return Result.Fail<School>("School Not Found!");
            }

            var validationResult = ValidateSchool(updatedSchool);
            if (validationResult.Failure){
                return Result.Fail<School>(validationResult.Error);
            }

            try {

            school.Name = updatedSchool.Name;
            school.Type = updatedSchool.Type;
            school.Capacity = updatedSchool.Capacity;
            school.Unit = updatedSchool.Unit;

            _context.SaveChanges();
            }
            catch (Exception ex) {

                return Result.Fail<School>(ex.Message);
            }

            return Result.Ok(school);
        }

        public Result RemoveById(int id) {
            var school = _context.Schools.Find(id);

            if (school == null) {
                return Result.Fail<School>("School Not Found");
            }

            if (_context.Students.Any(x => x.SchoolId == id)) {
                return Result.Fail<School>("Fail To Delete. Still Students In The School!");
            }

            try {

            _context.Schools.Remove(school);
            _context.SaveChanges();
            }
            catch (Exception ex) {

                return Result.Fail<School>(ex.Message);
            }

            return Result.Ok(school);
        }

        private Result ValidateSchool(School school) {

            if (!ValidateSchoolNameUniqueness(school)) {
                return Result.Fail<School>("School Name Already Exists!");
            }

            if (school.Capacity <= 0) {
                return Result.Fail<School>("Capacity Needs To Be More Than Zero!");
            }

            return Result.Ok();
        }

        private bool ValidateSchoolNameUniqueness(School school) {
            return !_context.Schools.Any(x =>
                x.Id != school.Id &&
                x.Name.ToLower() == school.Name.ToLower()
                //x.Unit.ToLower() == school.Unit.ToLower()
            );
        }
    }
}
