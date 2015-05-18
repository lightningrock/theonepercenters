using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOnePercents.Domain.ViewModel.Email
{
    public class SendResetPasswordEmailRequest : SendEmailRequestBase
    {
        public SendResetPasswordEmailRequest() {}

        public Guid CompanyID { get; set; }
        public Guid SettingID { get; set; }
        public Guid UserID { get; set; }
        public Guid PasswordSetIdentifier { get; set; }
        public string Application { get; set; }
    }
}
