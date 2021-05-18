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

            InitAdminData();
        }

        [TestCleanup]
        public void CleanData()
        {
            ClearUserData();

            ClearAdminData();
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_VALID_DATA_THEN_SUCCESS()
        {
            var model = InitUserModel();
            var result = Helper.UserPost<UserModel>(_url, dataInit);
            Assert.AreEqual(model.Email, result.Data.Email);
            Assert.AreEqual(model.FirstName, result.Data.FirstName);
            Assert.AreEqual(model.LastName, result.Data.LastName);
            Assert.AreEqual(model.PositionId, result.Data.PositionId);
            Assert.AreEqual(model.Sex, result.Data.Sex);
            Assert.AreEqual(model.DoB, result.Data.DoB);
            Assert.AreEqual(model.PhoneNo, result.Data.PhoneNo);
            Assert.AreEqual(model.RoleId, result.Data.RoleId);
        }

        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_MODEL_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel = null;
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
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
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
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
            expectedModel.Email = "asdfghjklqwertyuiopasdfghjklzxcvbnmlsdje3@gmail.com";
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
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
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
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
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
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
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
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
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_FIRST_NAME_MAX_LENGTH", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_FIRST_NAME_CHARACTER_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.FirstName = "28Shd&*(^";
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_FIRST_NAME_CHARACTERS", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_LAST_NAME_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.LastName = null;
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
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
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
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
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_LAST_NAME_MAX_LENGTH", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_LAST_NAME_CHARACTER_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.LastName = "28Shd&*(^";
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_LAST_NAME_CHARACTERS", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }
        [TestMethod]
        public void ADMIN_ADD_USER_WHEN_INVALID_PHONE_LENGTH_THEN_ERROR()
        {
            var expectedModel = InitUserModel();
            expectedModel.LastName = "098765432123645";
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
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
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
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
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
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
            var actualModel = Helper.UserPost<UserModel>(_url, expectedModel);
            dataInit.Add(actualModel.Data);
            var errorMess = JsonConvert.DeserializeObject<string>(actualModel.Error.Message);
            Assert.AreEqual("INVALID_MODEL_ROLE_NULL", errorMess);
            Assert.IsFalse(actualModel.IsSuccess);
            Assert.IsNotNull(actualModel.Error);
        }

        [TestMethod]
        public void USER_ADD_USER_HAVE_NO_PERMISSION_THEN_ERROR()
        {
        }

        #region Init User
        private void InitUserData()
        {
            var userModel = InitUserModel();
            var createResult = Helper.UserPost<UserModel>(_url, userModel);
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
                Helper.UserDelete<bool>(uri);
            });
        }
        private UserModel InitUserModel()
        {
            var model = new UserModel
            {
                Email = "Email: " + rand(),
                FirstName = "Username: " + rand(),
                LastName = "Lastname: " + rand(),
                PositionId = (int)ProjectRoles.MEMBER,
                Sex = SexEnum.FEMALE,
                DoB = DateTime.Today.AddDays(-new Random().Next(20 * 635)),
                PhoneNo = "Phone: " + rand(),
                RoleId = (int)Roles.USER,
            };
            return model;
        }
        #endregion

        #region Init Admin
        private void InitAdminData()
        {
            var userModel = InitAdminModel();
            var createResult = Helper.AdminPost<UserModel>(_url, userModel);
            if (createResult.Error == null)
            {
                dataInit.Add(createResult.Data);
            }
        }
        private void ClearAdminData()
        {
            dataInit.ForEach(x =>
            {
                var uri = _url + "/" + x;
                Helper.AdminDelete<bool>(uri);
            });
        }
        private UserModel InitAdminModel()
        {
            var model = new UserModel
            {
                Email = "Email: " + rand(),
                FirstName = "Username: " + rand(),
                LastName = "Lastname: " + rand(),
                PositionId = (int)ProjectRoles.MEMBER,
                Sex = SexEnum.FEMALE,
                DoB = DateTime.Today.AddDays(-new Random().Next(20 * 635)),
                PhoneNo = "Phone: " + rand(),
                RoleId = (int)Roles.ADMINISTRATOR,
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
