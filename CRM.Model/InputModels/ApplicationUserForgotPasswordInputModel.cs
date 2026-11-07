namespace CRM.Model.InputModels
{
    public class ApplicationUserForgotPasswordInputModel : ApplicationUserConfirmEmailInputModel
    {
        public string? Password { get; set; }

    }
}
