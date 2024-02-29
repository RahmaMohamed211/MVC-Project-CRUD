using System.ComponentModel.DataAnnotations;

namespace company.PL.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="First Name is Required")]
        public string FName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage ="Invaild Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is Required")]
        [DataType(DataType.Password)] //**********
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is Required")]
       
        //**********
        [Compare("Password",ErrorMessage = " confirm Password does not Match Password")]
        public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }
    }

}
