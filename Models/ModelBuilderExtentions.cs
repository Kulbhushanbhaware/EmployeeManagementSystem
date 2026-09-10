using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtentions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                 new Employee
                 {
                     Id = 1,
                     Name = "Mary",
                     Department = Dept.IT,
                     Email = "Mary@Cloverinfotech.com",
                     Photopath = "default/path1.jpg"
                 },
                   new Employee
                   {
                       Id = 2,
                       Name = "Sachin",
                       Department = Dept.HR,
                       Email = "sachin@Cloverinfotech.com",
                       Photopath = "default/path2.jpg"
                   });
        }
    }
}
