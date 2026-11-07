using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace CRM.Utility.IUtility
{
    public interface IApplicationEmailSender
    {
        Task SendEmailAsync(MailMessage message);
    }
}
