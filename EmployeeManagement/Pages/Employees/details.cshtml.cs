using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Employ.Model;
using Employ.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        private readonly IEmployeeRepository employeeRepository;
        public DetailsModel(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public Employee Employee { get; private set; }
        [TempData]
        public string Message { get; set; }

        public IActionResult OnGet(int id)
        {
             Employee = employeeRepository.GetEmployee(id);

            if (Employee == null)
                return RedirectToPage("pagenotfound");
            else
                return Page();
        }
    }
}
