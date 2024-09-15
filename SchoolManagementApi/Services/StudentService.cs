using SchoolManagementApi.Contexts;
using SchoolManagementApi.Models;
using SchoolManagementApi.Utils;

namespace SchoolManagementApi.Services {
    public class StudentService {

        private readonly DataBaseContext _context;

        public StudentService(DataBaseContext context) {
            _context = context;
        }

        public Result<Student> Create(Student student) {
            if (!ValidateSchoolExistance(student.SchoolId)) {
                return Result.Fail<Student>("School Not Found!");
            }

            if (!ValidatesSchoolCapacity(student.SchoolId)) {
                return Result.Fail<Student>("School Out Of Capacity!");
            }

            try {

                student = _context.Students.Add(student).Entity;
                _context.SaveChanges();
            }
            catch (Exception ex) {

                return Result.Fail<Student>(ex.Message);
            }

            return Result.Ok(student);
        }

        public Result<Student> GetById(int id) {
            var student = _context.Students.Find(id);
            if (student == null) {
                return Result.Fail<Student>("Student Not Found!");
            }

            return Result.Ok(student);
        }

        public IQueryable<Student> GetStudentsBySchoolId(int schoolId) => _context.Students.Where(
                student => student.SchoolId == schoolId
            );

        public IQueryable<Student> GetAll() => _context.Students;

        public Result<Student> Update(Student updatedStudent) {
            var student = _context.Students.Find(updatedStudent.Id);

            if (student == null) {
                return Result.Fail<Student>("Student Not Found!");
            }

            if (!ValidateSchoolExistance(updatedStudent.SchoolId)) {
                return Result.Fail<Student>("School Not Found"!);
            }

            if (updatedStudent.SchoolId != student.SchoolId && !ValidatesSchoolCapacity(updatedStudent.SchoolId)) {
                return Result.Fail<Student>("School Out Of Capacity!");
            }

            try {

                student.Name = updatedStudent.Name;
                student.LastName = updatedStudent.LastName;
                student.SchoolId = updatedStudent.SchoolId;

                _context.SaveChanges();
            }
            catch (Exception ex) {

                return Result.Fail<Student>(ex.Message);
            }

            return Result.Ok(student);
        }

        public Result RemoveById(int id) {
            var student = _context.Students.Find(id);

            try {

                if (student == null) {
                    return Result.Fail<Student>("Student Not Found!");
                }

                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            catch (Exception ex) {

                return Result.Fail<Student>(ex.Message);
            }

            return Result.Ok();
        }

        private bool ValidateSchoolExistance(int id) => _context.Schools.Any(x => x.Id == id);

        private bool ValidatesSchoolCapacity(int schoolId) {
            var school_capacity = _context.Schools.Where(x => x.Id == schoolId)
                                                  .Select(x => x.Capacity)
                                                  .FirstOrDefault();

            var studentsCount = _context.Students.Count(x => x.SchoolId == schoolId);

            return school_capacity > studentsCount;
        }
    }
}
