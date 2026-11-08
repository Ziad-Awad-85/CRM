using CRM.Model.ApplicationModels;
using CRM.Model.IdentityModels;
using CRM.Model.InputModels;
using CRM.Model.ViewModels;
using CRM.Utility;
using CRM.Utility.IUtility;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace CRM.Service.Identity
{
    public class AuthenticationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IApplicationEmailSender applicationEmailSender,
        ITokenHandler tokenHandler
        ) : IAuthenticationService
    {
        public async Task<ResponseModel<ApplicationUserProfileViewModel>> LoginAsync(ApplicationUserLoginInputModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                var claims = new List<Claim>
                {
                    new Claim(TokenParameters.UserId, user?.Id!),
                    new Claim(TokenParameters.Email, user?.Email!)
                };
                var token = tokenHandler.GenerateJwtToken(claims);

                return new ResponseModel<ApplicationUserProfileViewModel>
                {
                    IsSuccess = true,
                    Message = "Login successful",
                    Data = new ApplicationUserProfileViewModel(user!) { Token = token}
                };
            }

            string errorMessage = result.IsLockedOut ? "User account is locked out." :
                                  result.IsNotAllowed ? "User is not allowed to sign in." :
                                  result.RequiresTwoFactor ? "Two-factor authentication is required." :
                                  "Invalid login attempt.";
            return new ResponseModel<ApplicationUserProfileViewModel>
            {
                IsSuccess = false,
                Message = errorMessage,
                Data = null
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

        public async Task<ResponseModel<bool>> ConfirmEmailVerifyCodeAsync(ApplicationUserVerificationBaseInputModel model)
        {
            ArgumentNullException.ThrowIfNull(model.Email);
            ArgumentNullException.ThrowIfNull(model.Code);

            var user = userManager.FindByEmailAsync(model.Email).Result;
            if (user is null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            bool isCodeValid = user.VerificationCode != null && user.VerificationCode.ToString() == model.Code;
            if (isCodeValid)
            {
                user.EmailConfirmed = true;
                user.Activity = true;
                var result = await userManager.UpdateAsync(user);
                return new ResponseModel<bool>
                {
                    IsSuccess = result.Succeeded,
                    Message = result.Succeeded ? "Email confirmed successfully" : "Failed to confirm email"
                };
            }
            else
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "Invalid verification code"
                };
            }

        }

        public async Task<ResponseModel<bool>> ForgotPasswordAsync(ApplicationUserForgotPasswordInputModel model)
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

        public async Task<ResponseModel<bool>> ChangePasswordAsync(ApplicationUserRegisterInputModel model)
        {
            throw new NotImplementedException();
        }



        public async Task<ResponseModel<bool>> RefreshTokenAsync(ApplicationUserRegisterInputModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<bool>> ResetPasswordAsync(ApplicationUserForgotPasswordInputModel model)
        {
            ArgumentNullException.ThrowIfNull(model.Email);
            ArgumentNullException.ThrowIfNull(model.Code);
            ArgumentNullException.ThrowIfNull(model.Password);

            var user = userManager.FindByEmailAsync(model.Email).Result;
            if (user is null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            bool isCodeValid = user.VerificationCode != null && user.VerificationCode.ToString() == model.Code;
            if (isCodeValid)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                return new ResponseModel<bool>
                {
                    IsSuccess = result.Succeeded,
                    Message = result.Succeeded ? "Password reset successfully" : "Failed to reset password"
                };
            }
            else
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "Invalid verification code"
                };
            }
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
