using Employ.Model;
using System;
using System.Collections.Generic;

namespace Employ.services
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees();

        Employee GetEmployee(int id);

        Employee Update(Employee updatedEmployee);

        Employee Add(Employee newEmployee);

        Employee Delete(int id);
        IEnumerable<DeptHeadCount> EmployeeCountByDept(Dept? dept);

        IEnumerable<Employee> Search(string searchTerm);
    }
}

