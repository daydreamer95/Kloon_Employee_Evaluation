using Kloon.EmployeePerformance.Models.Criteria;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Kloon.EmployeePerformance.Test
{
    [TestClass]
    public class CriteriaTest : TestBase
    {
        private readonly Random _rand = new Random();
        const string _url = "/Criteria";
        public List<Guid> dataInit = new List<Guid>();

        [TestMethod]
        public void Criteria_GetAll_When_VailidData_Then_Success()
        {
            InitData();
            var result = Helper.UserGet<List<CriteriaModel>>(_url);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count > 0);
            Clear();
        }

        private CriteriaModel BuildCriteriaModel(Guid typeId)
        {
            var model = new CriteriaModel
            {
                Name = "Criteria Name " + rand(),
                TypeId = typeId,
                Description = "Criteria DesCription " + rand()
            };

            return model;
        }

        

        private void InitData()
        {
            var criteriaType = BuildCriteriaTypeModel();
            var createTypeResult = Helper.UserPost<CriteriaModel>(_url, criteriaType);
            if (createTypeResult.Error == null)
            {
                dataInit.Add(createTypeResult.Data.Id);
                var criteriaModel = BuildCriteriaModel(createTypeResult.Data.Id);
                var createResult = Helper.UserPost<CriteriaModel>(_url, criteriaModel);
                if (createResult.Error == null)
                {
                    dataInit.Add(createResult.Data.Id);
                }
            }
        }

        private void Clear() 
        {
            dataInit.ForEach(x => {
                var uri = _url + "/" + x;
                Helper.UserDelete<bool>(uri);
            });
        }

        private CriteriaModel BuildCriteriaTypeModel()
        {
            var model = new CriteriaModel
            {
                Name = "Criteria Type " + rand(),
                TypeId = null,
                Description = "Criteria Type DesCription " + rand()
            };

            return model;
        }

        private int rand() 
        {
            return _rand.Next(1, 100000);
        }
    }
}
