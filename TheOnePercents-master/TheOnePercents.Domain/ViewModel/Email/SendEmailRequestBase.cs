using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheOnePercents.Domain
{
    public class SendEmailRequestBase
    {
        public string RecipientsFullName { get; set; }
        public string RecipientsEmailAddress { get; set; }
        public string Subject { get; set; }

        public string FromName { get; set; }
        public string FromEmailAddress { get; set; }
    }
}
