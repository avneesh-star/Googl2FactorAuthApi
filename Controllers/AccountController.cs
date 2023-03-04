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

namespace google2fa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var response = await _accountService.CheckLogin(loginDto);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return Ok(new Response(false, ex.Message));
            }
        }

        [HttpGet("tfa/enable")]
        public async Task<IActionResult> EnableTfa()
        {
            try
            {
                int Userid = User.GetUserId();
                if (Userid <= 0)
                {
                    return Ok(new Response(false, "invalid token"));
                }
                string SecretKey = await _accountService.GetTfaKey(Userid);

                if (Request.HasValidTotp(SecretKey))
                {
                    var response = await _accountService.EnbleTfa(Userid);
                    return Ok(response);
                }
                else
                {
                    return Ok(new Response(false, "invalid otp"));
                }
            }
            catch (System.Exception ex)
            {
                return Ok(new Response(false, ex.Message));
            }
        }

    }
}