using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using google2fa.API.Helpers;
using google2fa.API.Helpers.Extensions;
using google2fa.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using google2fa.API.DTOs;
using google2fa.API.Data;
using Microsoft.EntityFrameworkCore;

namespace google2fa.API.Filters
{
    public class TwoFactorAuthorizeAttribute : Attribute, IResourceFilter
    {

        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var _dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
            int UserId = context.HttpContext.User.GetUserId();
            string SecretKey = _dbContext.UserMasters
            .Where(x => x.Id == UserId)
            .Select(u => u.TfaSecretKEy)
            .AsNoTracking()
            .FirstOrDefault();
            if (string.IsNullOrEmpty(SecretKey))
            {
                context.Result = new OkObjectResult(new Response(false, "invalid key"));
                return;
            }
            bool IsValidOtp = context.HttpContext.Request.HasValidTotp(SecretKey);
            if (!IsValidOtp)
            {
                context.Result = new OkObjectResult(new Response(false, "invalid otp"));
                return;
            }
        }
    }
}