using Kloon.EmployeePerformance.Models.Common;
using Kloon.EmployeePerformance.Models.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kloon.EmployeePerformance.Test.Api
{
    [TestClass]
    public class UserService_Test : TestBase
    {
        private readonly Random _rand = new Random();
        const string _url = "/User";
        public List<UserModel> dataInit = new List<UserModel>();

        [TestInitialize]
        public void InitData()
        {
            InitUserData();
        }

        [TestCleanup]
        public void CleanData()
        {
            ClearUserData();
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_VALID_DATA_THEN_SUCCESS()
        {
            var expectedModel = InitUserModel();
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            if (actualModel.Error == null)
            {
                dataInit.Add(actualModel.Data);
            }
            dataInit.Add(actualModel.Data);
            Assert.AreEqual(expectedModel.Email, actualModel.Data.Email);
            Assert.AreEqual(expectedModel.FirstName, actualModel.Data.FirstName);
            Assert.AreEqual(expectedModel.LastName, actualModel.Data.LastName);
            Assert.AreEqual(expectedModel.PositionId, actualModel.Data.PositionId);
            Assert.AreEqual(expectedModel.Sex, actualModel.Data.Sex);
            Assert.AreEqual(expectedModel.DoB, actualModel.Data.DoB);
            Assert.AreEqual(expectedModel.PhoneNo, actualModel.Data.PhoneNo);
            Assert.AreEqual(expectedModel.RoleId, actualModel.Data.RoleId);
        }

        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_MODEL_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel = null;
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_EMAIL_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.Email = null;
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_EMAIL_NULL", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_EMAIL_LENGTH_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.Email = "asdfghjklqwertyuiopasdfghjklzxcvbndfhzdhmlsdje3@kloon.vn";
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_EMAIL_MAX_LENGTH", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_EMAIL_WRONG_FORMAT_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.Email = "xcvbnmlsdje3@gmaildASfcoASfm";
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_EMAIL_FORMAT_WRONG", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_FIRST_NAME_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.FirstName = null;
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_FIRST_NAME_NULL", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_FIRST_NAME_MIN_LENGTH_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.FirstName = "a";
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_FIRST_NAME_MIN_LENGTH", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_FIRST_NAME_MAX_LENGTH_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.FirstName = "asdfghjklpoiuytrewqasf";
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_FIRST_NAME_MAX_LENGTH", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_LAST_NAME_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.LastName = null;
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_LAST_NAME_NULL", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_LAST_NAME_MIN_LENGTH_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.LastName = "a";
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_LAST_NAME_MIN_LENGTH", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_LAST_NAME_MAX_LENGTH_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.LastName = "asdfghjklpoiuytrewqasf";
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_LAST_NAME_MAX_LENGTH", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_PHONE_LENGTH_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.LastName = "098765432123645";
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_PHONE_MAX_LENGTH", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_POSITION_NULL_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.PositionId = 0;
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_POSITION_NULL", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_SEX_NULL_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.Sex = 0;
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_SEX_NULL", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_ROLE_NULL_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.RoleId = 0;
            var actualModel = Helper.AdminPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_ROLE_NULL", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }


        #region Init User
        private void InitUserData()
        {
            var userModel = InitUserModel();
            var createResult = Helper.AdminPost<UserModel>(_url, userModel);
            if (createResult.Error == null)
            {
                dataInit.Add(createResult.Data);
            }
        }
        private void ClearUserData()
        {
            dataInit.ForEach(x =>
            {
                var uri = _url + "/" + x;
                Helper.AdminDelete<bool>(uri);
            });
        }
        private UserModel InitUserModel()
        {
            var model = new UserModel
            {
                Email = "Email" + rand() + "@kloon.vn",
                FirstName = "Firstname " + rand(),
                LastName = "Lastname " + rand(),
                PositionId = new Random().Next(1, 3),
                Sex = (SexEnum)new Random().Next(1, 2),
                DoB = DateTime.Today.AddDays(-new Random().Next(20 * 635)),
                PhoneNo = "0" + rand(),
                RoleId = new Random().Next(1, 2),
            };
            return model;
        }
        #endregion

        #region Random func
        private int rand()
        {
            return _rand.Next(1, 1000000);
        }
        #endregion
    }
}
