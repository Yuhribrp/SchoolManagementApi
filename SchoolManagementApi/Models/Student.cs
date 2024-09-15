namespace SchoolManagementApi.Models {
    public class Student : Base {
        public String Name { get; set; }

        public String LastName { get; set; }

        public int SchoolId { get; set; }

        public School? School { get; set; }
    }
}
