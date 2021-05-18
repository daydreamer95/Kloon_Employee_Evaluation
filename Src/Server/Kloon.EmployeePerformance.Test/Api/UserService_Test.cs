using Kloon.EmployeePerformance.Models.Common;
using Kloon.EmployeePerformance.Models.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        public void Admin_Add_User_When_Valid_Data_Then_Success()
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
                PositionId = rand(),
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
                PositionId = rand(),
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
