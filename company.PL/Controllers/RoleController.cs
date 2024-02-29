using AutoMapper;
using company.BLL.Repositrios;
using company.PL.Helper;
using company.PL.ViewModels;
using Company.DAL.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace company.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public RoleController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index(string name)
        {

            if (string.IsNullOrEmpty(name))
            {

                var roles = await roleManager.Roles.Select(R => new RoleViewModel()
                {
                      Id = R.Id,
                      RoleName = R.Name,

                }).ToListAsync();
                return View(roles);
            }

            else
            {
                var roles = await roleManager.FindByNameAsync(name);
              
                    var MappedRole = new RoleViewModel()
                {
                   Id=roles.Id, 
                   RoleName = roles.Name,

                };
                return View(new List<RoleViewModel>() { MappedRole });
            }


        }
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel RoleVM)
        {
            if (ModelState.IsValid)
            {
               
                var MappedRole = mapper.Map<RoleViewModel, IdentityRole>(RoleVM);
                await roleManager.CreateAsync(MappedRole);


                
                return RedirectToAction(nameof(Index));
            }
            return View(RoleVM);
        }

        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var role = await roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();
            var Mappedrole = mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(ViewName, Mappedrole);
        }
        public async Task<IActionResult> Edit(string id)
        {


            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    //var Mappeduser = mapper.Map<UserViewModel, ApplicationUser>(UserVM);//deatached
                    var role = await roleManager.FindByIdAsync(id);
                    role.Name = roleVM.RoleName;
                  
                  
                    await roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //1.Log Execption
                    //2. friendly Message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(roleVM);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, RoleViewModel RoleVM)
        {
            if (id != RoleVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {

                    var role = await roleManager.FindByIdAsync(id);
                    await roleManager.DeleteAsync(role);

                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(RoleVM);
        }

        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null)
                return BadRequest();
            
            ViewBag.RoleId=roleId;
            var users = new List<UserInRoleViewModel>();

            foreach(var user in  await userManager.Users.ToListAsync())
            {
                var userInRole = new UserInRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,

                };
                if(await userManager.IsInRoleAsync(user,role.Name))
                    userInRole.IsSelected = true;
                else
                    userInRole.IsSelected = false;

                users.Add(userInRole);

            }
            return View(users);

        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(List<UserInRoleViewModel> users,string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role is null)
                return BadRequest();

          if(ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await userManager.FindByIdAsync(user.UserId);
                    if(appUser is not null)
                    {
                        if(user.IsSelected && !(await userManager.IsInRoleAsync(appUser,role.Name)))
                            await userManager.AddToRoleAsync(appUser, role.Name);

                        else if(!user.IsSelected && (await userManager.IsInRoleAsync(appUser, role.Name)))
                            await userManager.RemoveFromRoleAsync(appUser, role.Name);
                    }

                }
                return RedirectToAction(nameof(Edit),new {id =roleId});
            }
          return View(users);
        }
    }
}
