using System.ComponentModel.DataAnnotations;

namespace company.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)] //**********
        public string NewPassword { get; set; }

        [Required(ErrorMessage = " confirm Password is Required")]
        [DataType(DataType.Password)]
     
        [Compare("NewPassword", ErrorMessage = " confirm Password does not Match Password")]
        public string ConfirmPassword { get; set; }

        
    }
}
