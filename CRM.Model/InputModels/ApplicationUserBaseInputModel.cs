using CRM.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRM.Model.InputModels
{
    public class ApplicationUserBaseInputModel
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ImageName { get; set; }
    }
}
