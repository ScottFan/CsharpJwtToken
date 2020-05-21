using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft;
using CSRedis;

using System.Net.Http.Headers;

namespace JwtToken.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage CheckLogin(JObject postData)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            
            try
            {
                string _userCode = postData["UserCode"].ToString();
                string _password = postData["Password"].ToString();
                string _token = JwtHelper.GenerateToken(_userCode);
                Dictionary<string, object> result = new Dictionary<string, object>()
                {
                    { "Status","Ok"},
                    { "Token",_token}
                };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                httpResponseMessage.Content = new StringContent(json);
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpResponseMessage.Headers.Add("Authorization", _token);
                RedisClient redis = new RedisClient("192.168.197.132", 6379);
                redis.Set(_userCode, _token);
                return httpResponseMessage;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        [ApiTokenCheck]
        [HttpPost]
        public HttpResponseMessage VerifyToken()
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            Dictionary<string, object> result = new Dictionary<string, object>()
            {
                { "Status","当前Token验证通过"}
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            httpResponseMessage.Content = new StringContent(json);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpResponseMessage;
        }
    }
}
