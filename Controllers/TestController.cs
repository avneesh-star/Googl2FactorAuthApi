using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using google2fa.API.DTOs;
using google2fa.API.Services;
using Microsoft.AspNetCore.Authorization;
using google2fa.API.Helpers.Extensions;
using google2fa.API.Helpers;
using google2fa.API.Filters;

namespace google2fa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TestController : ControllerBase
    {
        public TestController()
        {

        }

        [HttpGet]
        [TwoFactorAuthorize]
        public IActionResult GetData()
        {
            try
            {
                var response = new Response(true, "opt valided successfully.");
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return Ok(new Response(false, ex.Message));
            }
        }
    }
}