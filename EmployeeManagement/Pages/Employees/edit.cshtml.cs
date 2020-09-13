using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employees.Model;
using Employees.services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace EmployeeManagement.Pages.Employees
{
    public class EditModel : PageModel
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        public EditModel(IEmployeeRepository employeeRepository,
                         IWebHostEnvironment webHostEnvironment)
        {
            this.employeeRepository = employeeRepository;
            // IWebHostEnvironment service allows us to get the
            // absolute path of WWWRoot folder
            this.webHostEnvironment = webHostEnvironment;
        }
        [BindProperty]
        public IFormFile Photo { get; set; }

        [BindProperty]
        public Employee Employee { get; set; }

        [BindProperty]
        public bool Notify { get; set; }

        public string Message { get; set; }
        public IActionResult OnGet(int?id)
        {
            // if id parameter has value, retrieve the existing
            // employee details, else create a new Employee
            if (id.HasValue)
            {
                Employee = employeeRepository.GetEmployee(id.Value);
            }
            else
            {
                Employee = new Employee();
            }

            if (Employee == null)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    // If a new photo is uploaded, the existing photo must be
                    // deleted. So check if there is an existing photo and delete
                    if (Employee.Photopath != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                            "images", Employee.Photopath);
                        System.IO.File.Delete(filePath);
                    }
                    // Save the new photo in wwwroot/images folder and update
                    // PhotoPath property of the employee object
                    Employee.Photopath = ProcessUploadedFile();
                }

                // If Employee ID > 0, call Update() to update existing 
                // employee details else call Add() to add new employee
                if (Employee.Id > 0)
                {
                    Employee = employeeRepository.Update(Employee);
                }
                else
                {
                    Employee = employeeRepository.Add(Employee);
                }
                return RedirectToPage("Index");
            }

            return Page();
        }
        public IActionResult OnPostUpdateNotificationPreferences(int id)
        {
            if (Notify)
            {
                Message = "Thank you for turning on notifications";
            }
            else
            {
                Message = "You have turned off email notifications";
            }
            TempData["message"] = Message;
            return RedirectToPage("Details", new { id });
        }

        private string ProcessUploadedFile()
            {
                string uniqueFileName = null;

                if (Photo != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    Photo.CopyTo(fileStream);
                }

                return uniqueFileName;
            }
        
    }
}
