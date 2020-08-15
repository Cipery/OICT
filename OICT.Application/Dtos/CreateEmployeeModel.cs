using System;

namespace OICT.Application.Dtos
{
    public class CreateEmployeeModel
    {
        public string Name { get; set; }
        public int ChildrenCount { get; set; }
        public bool Active { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime StartOfEmployment { get; set; }
    }
}
