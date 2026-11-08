using CRM.Model.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Model.ViewModels
{
    public class ApplicationUserProfileViewModel : ApplicationUserBaseViewModel
    {
        public string? FullName => $"{FirstName} {LastName}";
        public string? Token { get; set; }

        public ApplicationUserProfileViewModel()
        {
        }

        public ApplicationUserProfileViewModel(ApplicationUser model)
        {
            Id = model.Id;
            Email = model.Email;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Gender = model.Gender;
            DateOfBirth = model.DateOfBirth;
            RegistrationDate = model.RegistrationDate;
            ImageName = model.ImageName;
            Activity = model.Activity;
        }

    }
}
