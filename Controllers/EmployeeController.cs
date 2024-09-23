using Microsoft.AspNetCore.Mvc;
using MvcAdo.DAL;
using MvcAdo.Models;

namespace MvcAdo.Controllers
{
    public class EmployeeController:Controller
    {
        private readonly Employee_DAL _dal;

        public EmployeeController(Employee_DAL dal)
        {
            _dal =dal;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            List<Employee> employeeList = new List<Employee>();
            try{
                employeeList=_dal.GetAll();
                

            }
            catch( Exception ex)
            {
                TempData["errorMessage"]=ex.Message;
            }
                return View(employeeList);
        }

        public IActionResult CreateEmployee()
        {
            Employee emp = new Employee();
            emp.DateOfBirth=DateTime.Today;
            return View(emp);
        }
        [HttpPost]
        public IActionResult CreateEmployee(Employee model)
        {
            try{
            if(!ModelState.IsValid)
            {
                TempData["errorMessage"]="Model data is Invalid";
            }
            bool result = _dal.Create(model);
            if(!result)
            {
                TempData["errorMessage"]= "Unable to save the data";
                return View();

            }
            TempData["successMessage"]="Employee Saved";

            return RedirectToAction("GetAllEmployees");
            }
            catch(Exception ex){
                    TempData["errorMessage"]=ex.Message;
                    return View();
            }
           
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = new Employee();
         try{
                employee=_dal.GetById(id);
                

            }
            catch( Exception ex)
            {
                TempData["errorMessage"]=ex.Message;
            }
            return View(employee);

        }
        [HttpPost]
        public IActionResult Edit(Employee emp)
        {
            try{
                if(!ModelState.IsValid)
                {
                    TempData["errorMessage"]="Model Data invalid";

                }
                bool result = _dal.updateEmployee(emp);
                if(!result)
                {
                    TempData["errorMessage"]= "Invalid data filled";
                }
                TempData["successMessage"]="Employee data saved successfully";
                return RedirectToAction("GetAllEmployees");
            }
            catch(Exception ex){
                    TempData["errorMessage"]=ex.Message;
                    return View();
            }
        }
        public IActionResult Delete(int id)
        {
            bool result = _dal.delete(id);
            if(!result)
            {
                TempData["errorMessage"]= "error while deleting Employee";
            }
            TempData["successMessage"]= "employee deleted successfully";
            return RedirectToAction("GetAllEmployees");
        }
    }
}