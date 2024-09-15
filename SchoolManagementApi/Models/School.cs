namespace SchoolManagementApi.Models {
    public class School : Base {
        public String Name { get; set; }

        public SchoolTypeEnum Type { get; set; }

        public int Capacity { get; set; }

        public String? Unit { get; set; }

        public Guid? Identifier { get; set; }

        public List<Student>? Students { get; set; }
    }

    public enum SchoolTypeEnum {
        Private = 0,
        Public = 1
    }
}
