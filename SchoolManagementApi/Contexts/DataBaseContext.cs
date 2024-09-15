using SchoolManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagementApi.Contexts {
    public class DataBaseContext : DbContext {

        public DataBaseContext(DbContextOptions options) : base(options) {

        }

        public DbSet<School> Schools { get; set; }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            ConfigureSchool(modelBuilder);

            ConfigureStudent(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureSchool(ModelBuilder modelBuilder) {
            var schoolConfiguration = modelBuilder.Entity<School>().ToTable("School");

            schoolConfiguration.HasKey(x => x.Id);

            schoolConfiguration.Property(x => x.Name).IsRequired().HasMaxLength(250);

            schoolConfiguration.Property(x => x.Identifier).IsRequired().HasMaxLength(40);

            schoolConfiguration.Property(x => x.Type).IsRequired();

            schoolConfiguration.Property(x => x.Capacity).IsRequired();

            schoolConfiguration.Property(x => x.Unit).HasMaxLength(250);

            schoolConfiguration.HasMany(x => x.Students)
                                .WithOne(x => x.School)
                                .HasForeignKey(x => x.SchoolId)
                                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureStudent(ModelBuilder modelBuilder) {
            var studentConfiguration = modelBuilder.Entity<Student>().ToTable("Student");

            studentConfiguration.HasKey(x => x.Id);

            studentConfiguration.Property(x => x.Name).IsRequired().HasMaxLength(250);

            studentConfiguration.Property(x => x.LastName).IsRequired().HasMaxLength(250);

            studentConfiguration.HasOne(x => x.School)
                                .WithMany(x => x.Students)
                                .HasForeignKey(x => x.SchoolId)
                                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
