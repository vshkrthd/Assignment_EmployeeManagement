using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Library
{
    public class GetEmployeeResponse
    {
        public string Code { get; set; }
        public Pagination Meta { get; set; }
        public Employee Data { get; set; }
    }
}
