using EmployeeManagement.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EmployeeManagement.UnitTests.Models
{
    [TestClass]
    public class EmployeeTest
    {

        Employee _employee;

        [TestInitialize]
        public void TestInitialize()
        {
            _employee = new Employee()
            {
                Id = 1,
                Email = "test@domain.com",
                Gender = "Male",
                Name = "Rob Jhon",
                Status = "active"
            };


        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void CanGetSetPickup()
        {
            var actual = _employee;

            var expected = new Employee()
            {
                Id = 1,
                Email = "test@domain.com",
                Gender = "Male",
                Name = "Rob Jhon",
                Status = "active"
            };
            Assert.AreEqual(expected.Name, actual.Name);

            Assert.AreEqual(expected.GetType(), actual.GetType());
            Assert.AreEqual(expected.Id, actual.Id);
        }
    }
}
