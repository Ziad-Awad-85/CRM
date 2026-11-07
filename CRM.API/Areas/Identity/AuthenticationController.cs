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

        [HttpPost("forget-password")]
        [DisplayName("Forget Password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ApplicationUserRegisterInputModel model)
        {
            var response = await _authenticationService.ForgetPasswordAsync(model);
            return response ? Ok(response) : StatusCode(500);
        }

        [HttpPost("rest-password")]
        [DisplayName("Rest Password")]
        public async Task<IActionResult> RestPassword([FromBody] ApplicationUserRegisterInputModel model)
        {
            var response = await _authenticationService.RestPasswordAsync(model);
            return response ? Ok(response) : StatusCode(500);
        }

        [HttpPost("change-password")]
        [DisplayName("Change Password")]
        public async Task<IActionResult> ChangePassword([FromBody] ApplicationUserRegisterInputModel model)
        {
            var response = await _authenticationService.ChangePasswordAsync(model);
            return response ? Ok(response) : StatusCode(500);
        }

        [HttpPost("refresh-token")]
        [DisplayName("Refresh Token")]
        public async Task<IActionResult> RefreshToken([FromBody] ApplicationUserRegisterInputModel model)
        {
            var response = await _authenticationService.RefreshTokenAsync(model);
            return response ? Ok(response) : StatusCode(500);
        }
    }
}
