using CRM.Model.InputModels;
using CRM.Service.Iservice;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace CRM.API.Areas.Identity
{
    [Area("Identity")]
    [DisplayName("Authentication Controller")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService _authenticationService) : ControllerBase
    {
        [HttpPost("login")]
        [DisplayName("Login")]
        public async Task<IActionResult> Login([FromBody] ApplicationUserLoginInputModel model)
        {
            var response = await _authenticationService.LoginAsync(model);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPost("register")]
        [DisplayName("Register")]
        public async Task<IActionResult> Register([FromBody] ApplicationUserRegisterInputModel model)
        {
            var response = await _authenticationService.RegisterAsync(model);
            return response.IsSuccess ? Created() : BadRequest(response);
        }

        [HttpPost("confirm-email")]
        [DisplayName("Confirm Email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ApplicationUserConfirmEmailInputModel model)
        {
            var response = await _authenticationService.ConfirmEmailAsync(model);
            return response.IsSuccess ? Ok() : BadRequest(response);
        }

        [HttpPost("confirm-email-verify-code")]
        [DisplayName("Confirm Email Verify Code")]
        public async Task<IActionResult> ConfirmEmailVerifyCode([FromBody] ApplicationUserVerificationBaseInputModel model)
        {
            var response = await _authenticationService.ConfirmEmailVerifyCodeAsync(model);
            return response.IsSuccess ? Ok() : BadRequest(response);
        }

        [HttpPost("forgot-password")]
        [DisplayName("Forgot Password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ApplicationUserForgotPasswordInputModel model)
        {
            var response = await _authenticationService.ForgotPasswordAsync(model);
            return response.IsSuccess ? Ok(response) : StatusCode(500);
        }

        [HttpPost("reset-password")]
        [DisplayName("Reset Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ApplicationUserForgotPasswordInputModel model)
        {
            var response = await _authenticationService.ResetPasswordAsync(model);
            return response.IsSuccess ? Ok(response) : StatusCode(500);
        }

        [HttpPost("change-password")]
        [DisplayName("Change Password")]
        public async Task<IActionResult> ChangePassword([FromBody] ApplicationUserRegisterInputModel model)
        {
            var response = await _authenticationService.ChangePasswordAsync(model);
            return response.IsSuccess ? Ok(response) : StatusCode(500);
        }

        [HttpPost("refresh-token")]
        [DisplayName("Refresh Token")]
        public async Task<IActionResult> RefreshToken([FromBody] ApplicationUserRegisterInputModel model)
        {
            var response = await _authenticationService.RefreshTokenAsync(model);
            return response.IsSuccess ? Ok(response) : StatusCode(500);
        }
    }
}
