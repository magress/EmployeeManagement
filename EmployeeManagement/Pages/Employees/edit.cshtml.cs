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
        public Employee Employee { get; set; }

        [BindProperty]
        public bool Notify { get; set; }

        public string Message { get; set; }
        public IActionResult OnGet(int id)
        {
            Employee = employeeRepository.GetEmployee(id);

            if (Employee == null)
                return RedirectToPage("pagenotfound");
            else
                return Page();

        }

        public IActionResult OnPost(Employee employee)
        {

            if (Photo != null)
            {
                // If a new photo is uploaded, the existing photo must be
                // deleted. So check if there is an existing photo and delete
                if (employee.Photopath != null)
                {
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                        "images", employee.Photopath);
                    System.IO.File.Delete(filePath);
                }
                // Save the new photo in wwwroot/images folder and update
                // PhotoPath property of the employee object
                employee.Photopath = ProcessUploadedFile();
            }

            Employee = employeeRepository.Update(employee);
            return RedirectToPage("Index");
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
            return RedirectToPage("Details", new { id = id });
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
