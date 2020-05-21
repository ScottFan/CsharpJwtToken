using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using JWT.Exceptions;
using CSRedis;

namespace JwtToken
{
    public class JwtHelper
    {
        private const string secret = "scott";
        public static string GenerateToken(string userCode)
        {
            try
            {
                IDateTimeProvider provider = new UtcDateTimeProvider();
                var now = provider.GetNow();

                var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // or use JwtValidator.UnixEpoch

                var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds);

                //3分钟后失效
                var payload = new Dictionary<string, object>
                {
                    { "user",userCode},
                    { "exp",secondsSinceEpoch+ 60*3 },
                    { "jti",Guid.NewGuid() }
                };

                

                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

                var token = encoder.Encode(payload, secret);
                return token;
            }
            catch
            {
                return "";
            }
        }

        public static string VerifyToken(string Token)
        {
            string result = "";
            try
            {
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                var json = decoder.Decode(Token, secret, verify: true);//token为之前生成的字符串
                JObject jobj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                string userCode = jobj["user"].ToString();
                RedisClient redis = new RedisClient("192.168.197.132", 6379);
                string originalToken = redis.Get(userCode);
                if(!Token.Equals(originalToken))
                {
                    result = "该帐号在其它地方登录";
                }
            }
            catch (TokenExpiredException)
            {
                result = "Token 过期";
            }
            catch (SignatureVerificationException)
            {
                result = "Token 不正确";
            }
            catch (Exception ex)
            {
                result = "Token 验证出错";
            }
            return result;
        }
    }
}