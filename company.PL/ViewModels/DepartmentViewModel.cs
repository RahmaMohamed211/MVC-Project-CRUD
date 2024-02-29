using Company.DAL.models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace company.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Code is Required !!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is Required !")]
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime DataOfcreation { get; set; }

        public ICollection<Employee> employees { get; set; } = new HashSet<Employee>();
        //navigation proparty [many]
    }
}
