using System.ComponentModel.DataAnnotations;

namespace company.PL.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invaild Email")]
        public string Email { get; set; }
    }
}
