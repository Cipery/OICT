using System;
using System.ComponentModel.DataAnnotations;

namespace OICT.Application.Dtos
{
    public class UpdateEmployeeModel
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChildrenCount { get; set; }
        public bool Active { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime StartOfEmployment { get; set; }
    }
}
