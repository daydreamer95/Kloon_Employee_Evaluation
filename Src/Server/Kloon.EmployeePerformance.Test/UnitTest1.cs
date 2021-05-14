using Kloon.EmployeePerformance.Models.Criteria;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kloon.EmployeePerformance.Test
{
    [TestClass]
    public class UnitTest1 : TestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            Helper.UserGet<CriteriaModel>("");
            Helper.AdminGet<CriteriaModel>("");
            Helper.AdminPost<CriteriaModel>("", new object { });
        }
    }
}
