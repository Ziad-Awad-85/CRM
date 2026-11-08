using CRM.Model.ApplicationModels;
using CRM.Model.IdentityModels;
using CRM.Model.InputModels;

namespace CRM.Service.Identity
{
    public interface IUserService
    {
        Task<ResponseModel<bool>> UpdateProfileAsync(ApplicationUserProfileInputModel model, ApplicationUserContext applicationUserContext);
        Task<ResponseModel<bool>> ChangePasswordAsync(ApplicationUserChangePasswordInputModel model, ApplicationUserContext applicationUserContext);

    }
}
