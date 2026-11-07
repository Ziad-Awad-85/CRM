using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Utility
{
    public class EmailConfiguration
    {
       public required string  FromName { get; set; }
       public required string FromEmail { get; set; }
       public required string SmtpServer { get; set; }
       public required int Port { get; set; }
       public required string Username { get; set; }
       public required string Password { get; set; }
    }
}
