using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Model.ApplicationModels
{
    public class AuthenticationTokens
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsRefreshTokenValid { get; set; }

    }
}
