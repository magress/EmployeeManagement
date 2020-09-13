using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employees.Model;
using Employees.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Pages.Employees
{
    public class DeleteModel : PageModel
    {
        private readonly IEmployeeRepository employeeRepository;

        public DeleteModel(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [BindProperty]
        public Employee Employee { get; set; }
        public IActionResult OnGet(int id)
        {
            Employee = employeeRepository.GetEmployee(id);
            if(Employee==null)
            {
                return RedirectToPage("pagenotfound");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            Employee deleteemployee = employeeRepository.Delete(Employee.Id);
            if (deleteemployee == null)
            {
                return RedirectToPage("pagenotfound");
            }
            return RedirectToPage("Index");
        }
    }
}
