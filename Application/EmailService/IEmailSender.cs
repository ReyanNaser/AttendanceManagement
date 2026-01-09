using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EmailService
{
    public interface IEmailSender
    {
        public Task SenEmailAsync(string email, string subject, string body);
    }
}
