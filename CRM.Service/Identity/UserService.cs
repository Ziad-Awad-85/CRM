using CRM.Model.ApplicationModels;
using CRM.Model.IdentityModels;
using CRM.Model.InputModels;
using CRM.Model.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Service.Identity
{
    public class UserService(UserManager<ApplicationUser> userManager) : IUserService
    {
        public async Task<ResponseModel<bool>> ChangePasswordAsync(ApplicationUserChangePasswordInputModel model, ApplicationUserContext userContext)
        {
            ArgumentNullException.ThrowIfNull(userContext.UserId);
            ArgumentNullException.ThrowIfNull(model.CurrentPassword);
            ArgumentNullException.ThrowIfNull(model.NewPassword);

            var user = await userManager.FindByIdAsync(userContext.UserId) ?? throw new Exception("Unable to get user");

            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            return new ResponseModel<bool>
            {
                IsSuccess = result.Succeeded,
                Message = result.Succeeded ? "Password changed successfully" : "Unable to change password",
            };
        }

        public async Task<ResponseModel<ApplicationUserProfileViewModel>> GetUserProfileAsync(ApplicationUserContext userContext)
        {
            ArgumentNullException.ThrowIfNull(userContext.UserId);
            var user = await userManager.FindByIdAsync(userContext.UserId) ?? throw new Exception("Unable to get user");
            return new ResponseModel<ApplicationUserProfileViewModel>
            {
                IsSuccess = true,
                Message = "User profile retrieved successfully",
                Data = new ApplicationUserProfileViewModel(user)
            };
        }

        public async Task<ResponseModel<bool>> UpdateProfileAsync(ApplicationUserProfileInputModel model, ApplicationUserContext userContext)
        {
            ArgumentNullException.ThrowIfNull(userContext.UserId);
            ArgumentNullException.ThrowIfNull(model.FirstName);
            ArgumentNullException.ThrowIfNull(model.LastName);

            var user = await userManager.FindByIdAsync(userContext.UserId) ?? throw new Exception("Unable to get user");
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            if (model.DateOfBirth.HasValue)
                user.DateOfBirth = model.DateOfBirth.Value;
            if (model.Gender.HasValue)
                user.Gender = model.Gender.Value;
            if (!string.IsNullOrEmpty(model.ImageName))
                user.ImageName = model.ImageName;

            var result = await userManager.UpdateAsync(user);
            return new ResponseModel<bool>
            {
                IsSuccess = result.Succeeded,
                Message = result.Succeeded ? "Profile updated successfully" : "Unable to update profile",
            };
        }
    }
}
