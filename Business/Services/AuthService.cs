using Core.Abstracts.IServices;
using Core.Concretes.DTOs;
using Core.Concretes.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Responses;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Customer> userManager;
        private readonly SignInManager<Customer> signInManager;

        public AuthService(UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IResult> LoginAsync(LoginDto model)
        {
            // Model validasyonu
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                return Result.Failure("Kullanıcı adı ve şifre boş olamaz!", 400);
            }

            // Giriş denemesi
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return Result.Success();
            }
            else if (result.IsLockedOut)
            {
                // Hesap kilitli - çok fazla başarısız giriş denemesi
                return Result.Failure(
                    "Hesabınız güvenlik nedeniyle kilitlenmiştir. Lütfen daha sonra tekrar deneyin.",
                    403);
            }
            else if (result.IsNotAllowed)
            {
                // Kullanıcı giriş yapmaya izin verilmiyor (email onayı gerekli vb.)
                return Result.Failure(
                    "Bu hesapla giriş yapmaya izin verilmemiştir. Hesabınızı doğrulayın.",
                    403);
            }
            else if (result.RequiresTwoFactor)
            {
                // İki faktörlü kimlik doğrulama gerekli
                return Result.Failure(
                    "İki faktörlü kimlik doğrulama gereklidir.",
                    400);
            }
            else
            {
                // Genel başarısız giriş (yanlış kullanıcı adı veya şifre)
                return Result.Failure(
                    "Kullanıcı adı veya şifre yanlış!",
                    401);
            }
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public Task<IResult> RegisterAsync(RegisterDto model)
        {
            throw new NotImplementedException();
        }
    }
}
