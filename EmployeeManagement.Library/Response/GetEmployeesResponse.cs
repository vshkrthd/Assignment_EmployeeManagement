using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Library
{
    public class GetEmployeesResponse
    {
        public string Code { get; set; }
        public Pagination Meta { get; set; }
        public List<Employee> Data { get; set; }
    }
}
