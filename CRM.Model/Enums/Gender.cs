using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CRM.Model.Enums
{
    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male,
        [EnumMember(Value = "Female")]
        Female,
        [EnumMember(Value = "Other")]
        Other
    }
}
