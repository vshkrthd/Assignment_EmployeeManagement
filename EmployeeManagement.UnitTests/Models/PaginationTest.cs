using EmployeeManagement.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeeManagement.UnitTests.Models
{
    [TestClass]
    public class PaginationTest
    {
        Pagination _pagination;

        [TestInitialize]
        public void TestInitialize()
        {
            _pagination = new Pagination()
            {
                Limit = 10,
                Page = 3,
                Pages = 5,
                Total = 50
            };


        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void CanGetSetPickup()
        {
            var actual = _pagination;

            var expected = new Pagination()
            {
                Limit = 10,
                Page = 3,
                Pages = 5,
                Total = 50
            };
            Assert.AreEqual(expected.Limit, actual.Limit);

            Assert.AreEqual(expected.GetType(), actual.GetType());
            Assert.AreEqual(expected.Total, actual.Total);
        }
    }
}
