using Kloon.EmployeePerformance.Models.Criteria;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kloon.EmployeePerformance.Test
{
    [TestClass]
    public class CriteriaTest : TestBase
    {
        private readonly Random _rand = new Random();
        const string _url = "/Criteria";
        public List<CriteriaModel> dataInit = new List<CriteriaModel>();

        [TestInitialize]
        public void InitData()
        {
            InitCriteriaData();
        }

        [TestCleanup]
        public void CleanData()
        {
            Clear();
        }


        #region Get
        [TestMethod]
        public void Criteria_Admin_GetAll_When_VailidData_Then_Success()
        {
            var result = Helper.UserGet<List<CriteriaModel>>(_url);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count > 0);
        }

        [TestMethod]
        public void Criteria_Admin_GetAll_Search_When_VailidData_Then_Success()
        {
            var key = "";
            var item = dataInit.FirstOrDefault();
            if (item != null)
                key = item.Name;
            var url = $"{_url}?key={key}";
            var result = Helper.AdminGet<List<CriteriaModel>>(url);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count > 0);
        }

        [TestMethod]
        public void Criteria_User_GetAll_Search_When_VailidData_Then_Error()
        {
            var key = "";
            var item = dataInit.FirstOrDefault();
            if (item != null)
                key = item.Name;
            var url = $"{_url}?key={key}";
            var result = Helper.UserGet<List<CriteriaModel>>(url);
            Assert.IsNull(result.Data);
            Assert.IsTrue(result.Data.Count == 0);
        }

        [TestMethod]
        public void Criteria_User_GetAll_When_VailidData_Then_Error()
        {
            var result = Helper.UserGet<List<CriteriaModel>>(_url);
            Assert.IsNull(result.Data);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Data.Count == 0);
        }

        [TestMethod]
        public void Criteria_Admin_GetById_When_VailidData_Then_Success()
        {
            Guid id = Guid.Empty;
            var item = dataInit.FirstOrDefault();
            if (item != null)
                id = item.Id;
            var uri = $"{_url}/{id}";
            var result = Helper.AdminGet<CriteriaModel>(uri);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(item.Name, result.Data.Name);
        }

        [TestMethod]
        public void Criteria_Admin_GetById_When_InVailidData_Then_Return_Error()
        {
            Guid id = Guid.Empty;
            var uri = $"{_url}/{id}";
            var result = Helper.AdminGet<CriteriaModel>(uri);
            Assert.IsNotNull(result.Error);
            Assert.IsFalse(result.IsSuccess);
            var errorMess = JsonConvert.DeserializeObject<string>(result.Error.Message);
            Assert.AreEqual<string>("Invalid_Id", errorMess);
        }

        [TestMethod]
        public void Criteria_User_GetById_When_NoRole_Then_Error()
        {
            Guid id = Guid.Empty;
            var item = dataInit.FirstOrDefault();
            if (item != null)
                id = item.Id;
            var uri = $"{_url}/{id}";
            var result = Helper.UserGet<CriteriaModel>(uri);
            var errorMess = JsonConvert.DeserializeObject<string>(result.Error.Message);
            Assert.IsNotNull(result.Error);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("No Role", errorMess);
        }
        #endregion

        #region Add
        [TestMethod]
        public void Criteria_Admin_Add_When_ValidModel_Then_Sucssess()
        {
            Guid criteriaId = Guid.Empty;
            Guid typeId = Guid.Empty;
            var criteriaType = BuildCriteriaTypeModel();
            var createTypeResult = Helper.AdminPost<CriteriaModel>(_url, criteriaType);

            typeId = createTypeResult.Data.Id;
            dataInit.Add(createTypeResult.Data);
            var criteriaModel = BuildCriteriaModel(createTypeResult.Data.Id);
            var createResult = Helper.AdminPost<CriteriaModel>(_url, criteriaModel);
            if (createResult.Error == null)
            {
                dataInit.Add(createResult.Data);
                criteriaId = createResult.Data.Id;
            }

            var typeResult = Helper.AdminGet<CriteriaModel>($"{_url}/{typeId}");
            var criteriaResult = Helper.AdminGet<CriteriaModel>($"{_url}/{criteriaId}");

            Assert.AreEqual(criteriaType.Name, typeResult.Data.Name);

            Assert.AreEqual(criteriaModel.Name, criteriaResult.Data.Name);
        }

        [TestMethod]
        public void Criteria_Admin_Add_When_InValidModel_Then_Error()
        {
            Guid criteriaId = Guid.Empty;
            Guid typeId = Guid.Empty;
            var criteriaType = BuildCriteriaTypeModel();
            var createTypeResult = Helper.AdminPost<CriteriaModel>(_url, criteriaType);

            typeId = createTypeResult.Data.Id;
            dataInit.Add(createTypeResult.Data);
            var criteriaModel = BuildCriteriaModel(createTypeResult.Data.Id);

            // set Name = string.empty
            criteriaModel.Name = "";

            var createResult = Helper.AdminPost<CriteriaModel>(_url, criteriaModel);
            var errMess = JsonConvert.DeserializeObject<string>(createResult.Error.Message);
            Assert.AreEqual("Invalid_Mode", errMess);
            Assert.IsFalse(createResult.IsSuccess);
            Assert.IsNotNull(createResult.Error);
        }

        [TestMethod]
        public void Criteria_Admin_Add_When_DuplicateType_Then_Error()
        {
            Guid criteriaId = Guid.Empty;
            Guid typeId = Guid.Empty;
            var criteriaType = BuildCriteriaTypeModel();
            var createTypeResult = Helper.AdminPost<CriteriaModel>(_url, criteriaType);
            dataInit.Add(createTypeResult.Data);
            var criteriaModel = BuildCriteriaModel(createTypeResult.Data.Id);

            var typeDuplicate = BuildCriteriaTypeModel();
            // Set Duplicate Name
            typeDuplicate.Name = criteriaType.Name;
            var createTypeDuplicateResult = Helper.AdminPost<CriteriaModel>(_url, typeDuplicate);
            var errMess = JsonConvert.DeserializeObject<string>(createTypeDuplicateResult.Error.Message);

            Assert.AreEqual("Criteria Type Duplicate", errMess);
            Assert.IsFalse(createTypeDuplicateResult.IsSuccess);
            Assert.IsNotNull(createTypeDuplicateResult.Error);
        }

        [TestMethod]
        public void Criteria_Admin_Add_When_Duplicate_Criteria_Then_Error()
        {
            Guid criteriaId = Guid.Empty;
            Guid typeId = Guid.Empty;
            var criteriaType = BuildCriteriaTypeModel();
            var createTypeResult = Helper.AdminPost<CriteriaModel>(_url, criteriaType);
            dataInit.Add(createTypeResult.Data);
            var criteriaModel = BuildCriteriaModel(createTypeResult.Data.Id);

            var typeDuplicate = BuildCriteriaTypeModel();
            // Set Duplicate Name
            typeDuplicate.Name = criteriaType.Name;
            var createTypeDuplicateResult = Helper.AdminPost<CriteriaModel>(_url, typeDuplicate);
            var errMess = JsonConvert.DeserializeObject<string>(createTypeDuplicateResult.Error.Message);

            Assert.AreEqual("Criteria Duplicate", errMess);
            Assert.IsFalse(createTypeDuplicateResult.IsSuccess);
            Assert.IsNotNull(createTypeDuplicateResult.Error);
        }
        #endregion

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


        private void InitCriteriaData()
        {
            var criteriaType = BuildCriteriaTypeModel();
            var createTypeResult = Helper.AdminPost<CriteriaModel>(_url, criteriaType);

            if (createTypeResult.Error == null)
            {
                dataInit.Add(createTypeResult.Data);
                var criteriaModel = BuildCriteriaModel(createTypeResult.Data.Id);
                var createResult = Helper.AdminPost<CriteriaModel>(_url, criteriaModel);
                if (createResult.Error == null)
                    dataInit.Add(createResult.Data);
            }
        }

        private void Clear()
        {
            dataInit.ForEach(x =>
            {
                if (x.Id != Guid.Empty)
                {
                    var uri = _url + "/" + x;
                    Helper.UserDelete<bool>(uri);
                }
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
