using company.PL.Helper;
using company.PL.ViewModels;
using Company.DAL.models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Linq;
using System.Threading.Tasks;

namespace company.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMailSetting mailSetting;
        private readonly ISmsMessage smsMessage;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IMailSetting mailSetting,ISmsMessage smsMessage)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mailSetting = mailSetting;
            this.smsMessage = smsMessage;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) //server side validation
            {
                var User = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName = model.Email.Split("@")[0],
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree,
                };
                var Result = await userManager.CreateAsync(User, model.Password);


                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await userManager.FindByEmailAsync(model.Email);

                if (User is not null)
                {
                    var flag = await userManager.CheckPasswordAsync(User, model.Password);
                    if(flag)
                    {
                        var result = await signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe,false);
                        if(result.Succeeded)
                        {
                            return RedirectToAction("Index","Home");
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Password is Invaild");

                }
                ModelState.AddModelError(string.Empty, "Email is Invaild");
            }
            return View(model);
        }
        #endregion

        #region Sign Out
        public new async Task <IActionResult> Signout()
        { 
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region Forget password

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
          if (ModelState.IsValid)
          {
                
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user  is not null)
                {
                    var token =await userManager.GeneratePasswordResetTokenAsync(user); //uniqu for this time
                    var resetPasswordUrl= Url.Action("ResetPassword","Account",new {email=model.Email, Token=token},Request.Scheme);

                    //https://localhost:44371/Account/Resertpassword?email=rahma@gmail.com&token

                    var email = new Email()
                    {
                        Subject = "Reset You Password",
                        To= model.Email,
                        Body= resetPasswordUrl

                    };
                    //EmailSetting.SendEmail(email);
                    mailSetting.SendMail(email);
                    return RedirectToAction(nameof(checkYourInbox));

                }
                ModelState.AddModelError(string.Empty, "InValid Email");
             

          }
          return View(model);
        }




        //using sms
        //[HttpPost]
        //public async Task<IActionResult> SendSmS(ForgetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var user = await userManager.FindByEmailAsync(model.Email);
        //        if (user is not null)
        //        {
        //            var token = await userManager.GeneratePasswordResetTokenAsync(user); //uniqu for this time
        //            var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = model.Email, Token = token }, Request.Scheme);

        //            //https://localhost:44371/Account/Resertpassword?email=rahma@gmail.com&token

        //            var sms = new SMS()
        //            {
        //                PhoneNumber = user.PhoneNumber,
        //                Body = resetPasswordUrl
        //            };
        //            smsMessage.Send(sms);
        //            return Ok("Check Your Phone");


        //        }
        //        ModelState.AddModelError(string.Empty, "InValid Email");


        //    }
        //    return View(model);
        //}

        public IActionResult checkYourInbox()
        {
            return View();
        }
        #endregion

        #region Reset Password
        public IActionResult ResetPassword( string email,string Token)
        {
            TempData["email"] = email;
            TempData["token"] = Token;

            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var user = await userManager.FindByEmailAsync(email);
                var result = await userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return View(model);
        }

        #endregion
        //login with google 

        //public IActionResult GoogleLogin()
        //{
        //    var prop = new AuthenticationProperties
        //    {
        //        RedirectUri = Url.Action("GoogleResponse")
        //    };
        //    return Challenge(prop, GoogleDefaults.AuthenticationScheme);

        //}
        //public async Task<IActionResult> GoogleResponse()
        //{
        //    //Connect to google schema
        //    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        //    var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        //    {
        //        claim.Issuer,
        //        claim.OriginalIssuer,
        //        claim.Type,
        //        claim.Value

        //    });
        //    return RedirectToAction("Index", "Home");
        //}

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
