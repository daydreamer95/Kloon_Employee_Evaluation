using Kloon.EmployeePerformance.Models.Criteria;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Kloon.EmployeePerformance.Test
{
    [TestClass]
    public class UnitTest1 : TestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            var data = Helper.UserGet<List<CriteriaModel>>("/Criteria");
            Assert.AreEqual<int>(1, 1);
        }
    }
}
