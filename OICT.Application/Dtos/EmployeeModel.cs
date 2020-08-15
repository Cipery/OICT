using System;

namespace OICT.Application.Dtos
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChildrenCount { get; set; }
        public bool Active { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime StartOfEmployment { get; set; }
    }
}
