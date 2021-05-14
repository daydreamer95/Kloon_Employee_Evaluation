﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static Kloon.EmployeePerformance.Test.HttpExtension;

namespace Kloon.EmployeePerformance.Test
{
    public abstract class TestBase
    {
        private static Helper _helper;
        public Helper Helper
        {
            get
            {
                if (_helper == null)
                {
                    _helper = new Helper();
                    //_helper.GetTokenBase();
                }
                return _helper;
            }
        }
    }
    public class Helper
    {
        private readonly HttpClient _client;
        private readonly HeaderParam _headerparam;
        private static string _userToken;
        private static string _adminToken;
        private static string _token;
        public static TestSetting TestSettings { get; set; }
        //IServiceCollection services = new ServiceCollection();

        protected TestSetting _config
        {
            get; set;
        }
        public Helper()
        {
            InitToken();
            _config = new TestSetting { ApiUrl = "http://localhost:31102/" };
            _client = new HttpClient();
        }

        public void InitToken()
        {
            //var authenResult = Post<TestSetting>()
            _userToken = "hihi";
            _adminToken = "hihi";
        }

        public void GetToken(RoleType type)
        {
            switch (type)
            {
                case RoleType.ADMIN:
                    _headerparam.Token = _adminToken;
                    break;
                case RoleType.USER:
                    _headerparam.Token = _userToken;
                    break;
                default:
                    _headerparam.Token = _userToken;
                    break;
            }
        }

        public ResultModel<T> UserGet<T>(string url)
        {
            GetToken(RoleType.USER);
            return HttpExtension.Get<T>(_config, _headerparam, _client, url);
        }

        public ResultModel<T> UserPost<T>(string url, object model)
        {
            GetToken(RoleType.USER);
            return HttpExtension.Post<T>(_config, _headerparam, _client, url, model);
        }

        public ResultModel<T> UserPut<T>(string url, object model)
        {
            GetToken(RoleType.USER);
            return HttpExtension.Put<T>(_config, _headerparam, _client, url, model);
        }

        public ResultModel<T> UserDelete<T>(string url)
        {
            GetToken(RoleType.USER);
            return HttpExtension.Delete<T>(_config, _headerparam, _client, url);
        }

        public ResultModel<T> AdminGet<T>(string url)
        {
            GetToken(RoleType.USER);
            return HttpExtension.Get<T>(_config, _headerparam, _client, url);
        }

        public ResultModel<T> AdminPost<T>(string url, object model)
        {
            GetToken(RoleType.USER);
            return HttpExtension.Post<T>(_config, _headerparam, _client, url, model);
        }

        public ResultModel<T> AdminPut<T>(string url, object model)
        {
            GetToken(RoleType.USER);
            return HttpExtension.Put<T>(_config, _headerparam, _client, url, model);
        }

        public ResultModel<T> AdminDelete<T>(string url)
        {
            GetToken(RoleType.USER);
            return HttpExtension.Delete<T>(_config, _headerparam, _client, url);
        }

    }

    public class HeaderParam
    {
        public string Token { get; set; }

        public string SecurityKey { get; set; }

    }
    public static class HttpExtension
    {
        public static ResultModel<T> Get<T>(TestSetting config, HeaderParam headerParam, HttpClient client, string url)
        {
            SetupClient(client, headerParam, config);
            url = "api" + url;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;

            return ReadStringAsObject<T>(responseMessage);
        }

        public static ResultModel<T> Post<T>(TestSetting config, HeaderParam headerParam, HttpClient client, string url, object model)
        {
            SetupClient(client, headerParam, config);
            url = "api" + url;
            HttpResponseMessage responseMessage = client.PostAsJsonAsync(url, model).Result;

            return ReadStringAsObject<T>(responseMessage);
        }

        public static ResultModel<T> Put<T>(TestSetting config, HeaderParam headerParam, HttpClient client, string url, object model)
        {
            SetupClient(client, headerParam, config);
            url = "api" + url;
            HttpResponseMessage responseMessage = client.PutAsJsonAsync(url, model).Result;

            return ReadStringAsObject<T>(responseMessage);
        }

        public static ResultModel<T> Delete<T>(TestSetting config, HeaderParam headerParam, HttpClient client, string url)
        {
            SetupClient(client, headerParam, config);
            url = "api" + url;
            HttpResponseMessage responseMessage = client.DeleteAsync(url).Result;

            return ReadStringAsObject<T>(responseMessage);
        }

        private static void SetupClient(HttpClient client, HeaderParam headerParam, TestSetting config)
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri(config.ApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            if (headerParam == null)
                return;

            if (!string.IsNullOrEmpty(headerParam.Token))
                client.DefaultRequestHeaders.Add("Authorization", headerParam.Token);


            if (!string.IsNullOrEmpty(headerParam.SecurityKey))
                client.DefaultRequestHeaders.TryAddWithoutValidation("SecurityKey", config.SecurityKey);
        }
        private static ResultModel<T> ReadStringAsObject<T>(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<T>(content);
                return new ResultModel<T> { Data = data };
            }

            var message = responseMessage.Content.ReadAsStringAsync().Result;
            var error = StringToErrorJson<T>(message);
            error.StatusCode = (ErrorType)responseMessage.StatusCode;
            return new ResultModel<T>
            {
                Error = error
            };
        }
        private static ResultModel<T>.ErrorModel StringToErrorJson<T>(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return new ResultModel<T>.ErrorModel
                {
                    StatusCode = ErrorType.BAD_REQUEST, //ErrorCode.EXCEPTION
                    Message = string.Empty
                };
            }
            try
            {
                return JsonConvert.DeserializeObject<ResultModel<T>.ErrorModel>(message);
            }
            catch
            {
                return new ResultModel<T>.ErrorModel
                {
                    StatusCode = ErrorType.BAD_REQUEST, //ErrorCode.EXCEPTION
                    Message = message
                };
            }
        }
        public class ResultModel<T>
        {
            public T Data { get; set; }

            public ErrorModel Error { get; set; }

            public bool IsSuccess => Error == null;

            public class ErrorModel
            {
                public ErrorType StatusCode { get; set; }

                /// <summary>
                /// Get first message from messages array.
                /// It return Empty if Messages is null or empty.
                /// </summary>
                public string Message { get; set; }
            }
        }
        public enum ErrorType
        {
            NOT_AUTHORIZED,
            NOT_AUTHENTICATED,
            NO_ROLE,
            NO_DATA_ROLE,
            BAD_REQUEST,
            NOT_EXIST,
            DUPLICATED,
            CONFLICTED,
            INTERNAL_ERROR,
            CONFLICTED_ROLE_CHANGE
        }

        public enum RoleType
        {
            ADMIN,
            USER
        }
    }
}
