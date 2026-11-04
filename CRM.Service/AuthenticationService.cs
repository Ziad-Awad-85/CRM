using CRM.Model.IdentityModels;
using CRM.Model.InputModels;
using CRM.Service.Iservice;
using Microsoft.AspNetCore.Identity;

namespace CRM.Service
{
    public class AuthenticationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager
        ) : IAuthenticationService
    {
        public async Task<bool> ChangePasswordAsync(ApplicationUserRegisterInputModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ForgetPasswordAsync(ApplicationUserRegisterInputModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> LoginAsync(ApplicationUserLoginInputModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            return result.Succeeded ? true : throw new Exception($"Unable to login user, Errors: {result.ToString()}");

        }

        public async Task<bool> RefreshTokenAsync(ApplicationUserRegisterInputModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterAsync(ApplicationUserRegisterInputModel model)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model.Email);
            ArgumentException.ThrowIfNullOrWhiteSpace(model.Password);

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                RegistrationDate = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, model.Password);

            return result.Succeeded ? true : throw new Exception($"Unable to create user, Errors: {result.Errors}");
        }

        public async Task<bool> RestPasswordAsync(ApplicationUserRegisterInputModel applicationUserRestPasswordInput)
        {
            throw new NotImplementedException();
        }
    }
}
