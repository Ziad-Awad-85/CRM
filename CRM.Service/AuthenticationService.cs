using CRM.Model.ApplicationModels;
using CRM.Model.IdentityModels;
using CRM.Model.InputModels;
using CRM.Service.Iservice;
using CRM.Utility;
using CRM.Utility.IUtility;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net.Mime;

namespace CRM.Service
{
    public class AuthenticationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IApplicationEmailSender applicationEmailSender
        ) : IAuthenticationService
    {
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
        
        public async Task<ResponseModel<bool>> ConfirmEmailAsync(ApplicationUserConfirmEmailInputModel model)
        {
            ArgumentNullException.ThrowIfNull(model.Email);
            var user = userManager.FindByEmailAsync(model.Email).Result;
            if (user == null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    Data = false
                };
            }

            if (user.EmailConfirmed)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "Email already confirmed",
                    Data = false
                };
            }

            user.VerificationCode = GenerateVerificationCode();
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "Failed to save verification code"
                };
            }

            model.Code = user.VerificationCode.ToString();
            model.FullName = user.FullName!;
            await SendEmailConfirmationCodeAsync(model);

            return new ResponseModel<bool>
            {
                IsSuccess = true,
                Message = "Verification code sent successfully"
            };

        }



        public async Task<bool> ChangePasswordAsync(ApplicationUserRegisterInputModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ForgetPasswordAsync(ApplicationUserRegisterInputModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RefreshTokenAsync(ApplicationUserRegisterInputModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RestPasswordAsync(ApplicationUserRegisterInputModel applicationUserRestPasswordInput)
        {
            throw new NotImplementedException();
        } 
        
        private short? GenerateVerificationCode()
        {
            Random random = new();
            return (short)random.Next(1000, 9999);
        }

        private async Task SendEmailConfirmationCodeAsync(ApplicationUserConfirmEmailInputModel model)
        {
            MailMessage mailMessage = new();
            mailMessage.To.Add(model.Email);
            mailMessage.Subject = "CRM Application";

            var emailContent = model.EmailTemplate.Replace("{FullName}", model.FullName)
                                                  .Replace("{Code}", model.Code);
            var alternateView = AlternateView.CreateAlternateViewFromString(emailContent, null, MediaTypeNames.Text.Html);
            mailMessage.AlternateViews.Add(alternateView);
            await applicationEmailSender.SendEmailAsync(mailMessage);
        }
    }
}
