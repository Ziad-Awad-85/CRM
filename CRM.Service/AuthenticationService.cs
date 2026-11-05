using CRM.Model.ApplicationModels;
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

        public async Task<ResponseModel<bool>> LoginAsync(ApplicationUserLoginInputModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (result.Succeeded)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = true,
                    Message = "Login successful",
                    Data = true
                };
            }

            string errorMessage = result.IsLockedOut ? "User account is locked out." :
                                  result.IsNotAllowed ? "User is not allowed to sign in." :
                                  result.RequiresTwoFactor ? "Two-factor authentication is required." :
                                  "Invalid login attempt.";
            return new ResponseModel<bool>
            {
                IsSuccess = false,
                Message = errorMessage,
                Data = false
            };
        }

        public async Task<bool> RefreshTokenAsync(ApplicationUserRegisterInputModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<bool>> RegisterAsync(ApplicationUserRegisterInputModel model)
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

            if (result.Succeeded)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = true,
                    Message = "User created successfully",
                    Data = true
                };
            }

            string errorMessage = result.Errors.Any()
                ? string.Join("; ", result.Errors.Select(e => e.Code))
                : "Unable to register user due to unknown errors.";

            return new ResponseModel<bool>
            {
                IsSuccess = false,
                Message = errorMessage,
                Data = false
            };
        }

        public async Task<bool> RestPasswordAsync(ApplicationUserRegisterInputModel applicationUserRestPasswordInput)
        {
            throw new NotImplementedException();
        }
    }
}
