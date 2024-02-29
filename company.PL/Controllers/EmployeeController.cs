using AutoMapper;
using company.BLL.interfaces;
using company.PL.Helper;
using company.PL.ViewModels;
using Company.DAL.Context;
using Company.DAL.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace company.PL.Controllers
{
    [Authorize(Roles = "Admin,NormalUser")]
    public class EmployeeController : Controller
    {
     
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper )
        {
            
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public  async Task<IActionResult> Index(string SearchValue)
        { 
            IEnumerable<Employee> employees;
            if(string.IsNullOrEmpty(SearchValue))
                employees = await this.unitOfWork.EmployeeRepository.GetAll();
            else
                employees=this.unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);
            var MappedEmp = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(MappedEmp);
        }
        [HttpGet]
        public IActionResult Create()
        {
           // ViewData["Departmets"] = departmentRepository.GetAll();
           //ViewBag.Departments = departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        { 
            if(ModelState.IsValid)
            {
              
                employeeVM.ImageName = DocumentSettings.UploadImage(employeeVM.Image, "Images");
                var MappedEmp=mapper.Map<EmployeeViewModel,Employee>(employeeVM);
               
               await unitOfWork.EmployeeRepository.Add(MappedEmp);
                var count =  await unitOfWork.Compelete();
              


                if(count > 0)
                {
                    TempData["Message"] = "employee is created successfuly";
                   
                }
                else
                {
                    TempData["Message"] = "An Error has occured employee not created :(";

                   
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is  null)
                return BadRequest();
            var employee = await unitOfWork.EmployeeRepository.Get(id.Value);

            if(employee is null)
                return NotFound();
            var MappedEmp = mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(ViewName, MappedEmp);
        }
        [HttpGet]
        public async Task <IActionResult> Edit(int ? id)
        {

      
            return  await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id,EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid) {
                try
                {



                    if (employeeVM.Image is not null)
                    {
                        employeeVM.ImageName = DocumentSettings.UploadImage(employeeVM.Image, "Images");
                    }
                       
                        


                    
                   
                    var MappedEmp = mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    unitOfWork.EmployeeRepository.Update(MappedEmp);
                   var count=  await unitOfWork.Compelete();
                  
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //1.Log Execption
                    //2. friendly Message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(employeeVM);
        }

        [HttpGet]
        public async Task <IActionResult> Delete(int? id)
        {
            return  await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute]int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmp = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    if (MappedEmp.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(MappedEmp.ImageName, "Images");
                    }
                    unitOfWork.EmployeeRepository.Delete(MappedEmp);
                    await unitOfWork.Compelete();
                   
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }



    }
}
