using Company.DAL.models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace company.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required !")]
        [MaxLength(50,ErrorMessage = "Max Lenght of Name is 50 char")]
        [MinLength(5, ErrorMessage = "Min Lenght of Name is 5 char")]
        public string Name { get; set; }

        [Range(22, 30)]
        public int? Age { get; set; }

        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address must be like 123-street-City-Country")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Display(Name = "HireDate")]
        public DateTime HireDate { get; set; }
        public IFormFile Image { get; set; }

        public string ImageName { get; set; }
    
        public bool IsDeleted { get; set; }

       

        [ForeignKey("Department")]

        public int? DepartmentId { get; set; }
        public Department Department { get; set; } //navigation proparty one
    }
}
