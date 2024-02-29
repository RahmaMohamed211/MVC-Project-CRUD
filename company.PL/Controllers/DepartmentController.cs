using AutoMapper;
using company.BLL.interfaces;
using company.PL.ViewModels;
using Company.DAL.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace company.PL.Controllers
{
    [Authorize(Roles = "Admin,NormalUser")]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        // private  readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;
        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            //this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {//GetAll
            var departments = await unitOfWork.DepartmentRepository.GetAll();
            var MappedDept = mapper.Map<IEnumerable<Department>,IEnumerable<DepartmentViewModel>>(departments);
            return View(MappedDept);
        
        }
        [HttpGet]
        public IActionResult Create()
        {
       
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid) //server side validation
            {
                var MappedDept = mapper.Map<DepartmentViewModel, Department>(departmentVM);
                 await unitOfWork.DepartmentRepository.Add(MappedDept);
                int result = await unitOfWork.Compelete();
                if (result > 0)
                {
                    TempData["Message"] = "Department is created !";
                }
                else
                {
                    TempData["Message"] = "An Error has occured employee not created :(";


                }
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }
        public async  Task<IActionResult> Details( int? id,string ViewName= "Details")
        {
            if( id is null)
                return BadRequest();

            var department = await unitOfWork.DepartmentRepository.Get(id.Value);            

            if(department is null)
                return NotFound();
            var MappedDept = mapper.Map<Department, DepartmentViewModel>(department);
            return View(ViewName, MappedDept);

        }
        [HttpGet]
        public async  Task<IActionResult> Edit(int? id)
        {
            return await Details(id,"Edit");
            //if (id is null)
            //    return BadRequest();

            //var Department = departmentRepository.Get(id.Value);

            //if (Department is null)
            //    return NotFound();

            //return View(Department);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id,DepartmentViewModel departmentVM)
        { 
            if(id != departmentVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {

                try
                {
                    var MappedDept = mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    unitOfWork.DepartmentRepository.Update(MappedDept);
                   await unitOfWork.Compelete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //1.Log Execption
                    //2. friendly Message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVM); 
        
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            return  await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute]int id,DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();
            try
            {
                var MappedDept = mapper.Map<DepartmentViewModel, Department>(departmentVM);
                unitOfWork.DepartmentRepository.Delete(MappedDept);
               await unitOfWork.Compelete();
                return RedirectToAction(nameof (Index));
            }catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(departmentVM);
        }

    }
}
