using AutoMapper;
using company.BLL.interfaces;
using company.BLL.Repositrios;
using company.PL.Helper;
using company.PL.ViewModels;
using Company.DAL.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace company.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;

        public UserController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }
        public async  Task<IActionResult> Index(string emailSearch)
        {
            

            if (string.IsNullOrEmpty(emailSearch))
            {

                var users = await userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FName = U.FName,
                    LName = U.LName,
                    Email = U.Email,
                    PhoneNumber = U.PhoneNumber,
                    Roles = userManager.GetRolesAsync(U).Result

                }).ToListAsync();
                return View(users);
            }

            else
            {
              var  user = await userManager.FindByEmailAsync(emailSearch);
                var MappedUser = new UserViewModel()
                {
                    Id = user.Id,
                    FName = user.FName,
                    LName = user.LName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = userManager.GetRolesAsync(user).Result


                };
                // var MappedUser=mapper.Map<ApplicationUser, UserViewModel>(user);
                return View(new List<UserViewModel>() { MappedUser });
               // return View(MappedUser);
            }
                
            
        }



        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var users = await userManager.FindByIdAsync(id);

            if (users is null)
                return NotFound();
            var Mappeduser = mapper.Map<ApplicationUser, UserViewModel>(users);
            return View(ViewName, Mappeduser);
        }

        public async Task<IActionResult> Edit(string id)
        {
            

            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel UserVM)
        {
            if (id != UserVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    //var Mappeduser = mapper.Map<UserViewModel, ApplicationUser>(UserVM);//deatached
                    var user =await userManager.FindByIdAsync(id);
                    user.FName = UserVM.FName;
                   user.LName = UserVM.LName;
                    user.PhoneNumber = UserVM.PhoneNumber;
                    //user.Email = UserVM.Email;
                    //user.SecurityStamp =Guid.NewGuid().ToString();
                    await userManager.UpdateAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //1.Log Execption
                    //2. friendly Message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(UserVM);
        }
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, UserViewModel UserVM)
        {
            if (id != UserVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    
                  var user = await userManager.FindByIdAsync(id);
                    await userManager.DeleteAsync(user);
                  
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(UserVM);
        }

    }
}
