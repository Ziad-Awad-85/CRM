using CRM.Model.ApplicationModels;
using CRM.Model.InputModels;

namespace CRM.Service.Iservice
{
    public interface IAuthenticationService
    {
        Task<ResponseModel<bool>> LoginAsync(ApplicationUserLoginInputModel model);
        Task<ResponseModel<bool>> RegisterAsync(ApplicationUserRegisterInputModel model);
        Task<ResponseModel<bool>> ConfirmEmailAsync(ApplicationUserConfirmEmailInputModel model);
        Task<bool> ForgetPasswordAsync(ApplicationUserRegisterInputModel model);
        Task<bool> RestPasswordAsync(ApplicationUserRegisterInputModel model);
        Task<bool> ChangePasswordAsync(ApplicationUserRegisterInputModel model);
        Task<bool> RefreshTokenAsync(ApplicationUserRegisterInputModel model);
        
    }
}
