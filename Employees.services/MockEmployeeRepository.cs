using Employees.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Employees.services
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeelist;

        public MockEmployeeRepository()
        {
            _employeelist = new List<Employee>()
 {
                new Employee() { Id = 1, Name = "Mary", Department = Dept.HR,
                    Email = "mary@pragimtech.com", Photopath="mary.png" },
                new Employee() { Id = 2, Name = "John", Department = Dept.IT,
                    Email = "john@pragimtech.com", Photopath="john.png" },
                new Employee() { Id = 3, Name = "Sara", Department = Dept.IT,
                    Email = "sara@pragimtech.com", Photopath="sara.png" },
                new Employee() { Id = 4, Name = "David", Department = Dept.Payroll,
                    Email = "david@pragimtech.com" },
            };

        }
        IEnumerable<Employee> IEmployeeRepository.GetAllEmployees()
        {
            return _employeelist;
        }

        public Employee GetEmployee(int id)
        {
            return _employeelist.FirstOrDefault(e => e.Id == id);
        }

        public Employee Update(Employee updatedEmployee)
        {
            Employee employee = _employeelist.FirstOrDefault(e => e.Id == updatedEmployee.Id);

            if(employee!=null)
            {
                employee.Name = updatedEmployee.Name;
                employee.Email = updatedEmployee.Email;
                employee.Department = updatedEmployee.Department;
                employee.Photopath = updatedEmployee.Photopath;
            }

            return employee;
        }

        public Employee Add(Employee newEmployee)
        {
            newEmployee.Id = _employeelist.Max(e => e.Id) + 1;
            _employeelist.Add(newEmployee);
            return newEmployee;
        }
    }
}
