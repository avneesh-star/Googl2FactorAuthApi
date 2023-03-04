using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using google2fa.API.Data;
using google2fa.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace google2fa.API.Services
{
    public interface IAccountService
    {
        Task<Response> CheckLogin(LoginDto login);
        Task<Response> EnbleTfa(int UserId);
        Task<string> GetTfaKey(int UserId);
    }

    public class AccountService : IAccountService
    {
        private readonly AppDbContext context;
        private readonly ITokenService tokenService;

        public AccountService(AppDbContext context, ITokenService tokenService)
        {
            this.tokenService = tokenService;
            this.context = context;
        }

        public async Task<Response> CheckLogin(LoginDto login)
        {
            if (await context.UserMasters.AnyAsync(x => x.Email == login.Email && x.Password == login.Password))
            {
                var user = await context.UserMasters.FirstOrDefaultAsync(x => x.Email == login.Email && x.Password == login.Password);
                if (!user.TfaEnabled && string.IsNullOrEmpty(user.TfaSecretKEy))
                {
                    user.TfaSecretKEy = TimeSensitivePassCode.GeneratePresharedKey();
                    await context.SaveChangesAsync();
                }
                LogInResponse logInResponse = new LogInResponse
                {
                    Name = user.Name,
                    Email = user.Email,
                    SecretKey = user.TfaEnabled ? null : user.TfaSecretKEy,
                    Tfa = user.TfaEnabled,
                    AccessToken = tokenService.CreateToken(user.Id.ToString(), 1440),
                    ExpiresOn = DateTime.UtcNow.AddMinutes(1440)
                };
                return new Response(true, "Successful login.", logInResponse);
            }
            else
            {
                return new Response(false, "invalid email/password");
            }
        }

        public async Task<Response> EnbleTfa(int UserId)
        {
            var user = await context.UserMasters.FirstOrDefaultAsync(u => u.Id == UserId);
            if (user == null)
            {
                return new Response(false, "invalid user");
            }
            user.TfaEnabled = true;
            await context.SaveChangesAsync();
            return new Response(true, "Two-factor authentication was enabled successfully.");
        }

        public async Task<string> GetTfaKey(int UserId)
        {
            var user = await context.UserMasters.FirstOrDefaultAsync(u => u.Id == UserId);
            string key = user?.TfaSecretKEy;
            return key;
        }
    }
}