using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace JwtToken
{
    public class ApiTokenCheck : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext context)
        {
            var authHeader = context.Request.Headers.FirstOrDefault(a => a.Key == "Authorization");//获取接收的Token
            if (context.Request.Headers == null)
            {
                context.Response = context.Request.CreateErrorResponse(System.Net.HttpStatusCode.Unauthorized, new HttpError("Token 不正确"));
            }
            if(!context.Request.Headers.Any())
            {
                context.Response = context.Request.CreateErrorResponse(System.Net.HttpStatusCode.Unauthorized, new HttpError("Token 不正确"));
            }
            if(authHeader.Key == null)
            {
                context.Response = context.Request.CreateErrorResponse(System.Net.HttpStatusCode.Unauthorized, new HttpError("Token 不正确"));
            }
            var token = authHeader.Value.FirstOrDefault();
            string Verify = JwtHelper.VerifyToken(token);
            if(Verify != "")
            {
                context.Response = context.Request.CreateErrorResponse(System.Net.HttpStatusCode.Unauthorized, new HttpError(Verify));
            }
            
        }
    }
}