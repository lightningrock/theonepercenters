using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOnePercents.Domain;
using TheOnePercents.Infrastructure;
using TheOnePercents.Model;
using TheOnePercents.Repository;
using Xunit;

namespace TheOnePercents.Test._EmailTest_
{
    public class EmailTest
    {
        private readonly IRepositoryBase<theonepercentersEntities1> _repositoryBase;
        private readonly IEmailService _emailService;

        public EmailTest()
        {
            _repositoryBase = RepositoryDI.Resolve<IRepositoryBase<theonepercentersEntities1>>();
            _emailService = RepositoryDI.Resolve<IEmailService>();
        }

        [Fact]
        public void Shoul_Send_Test_Email() { 
            SendEmailRequestBase request = new SendEmailRequestBase();

            request.RecipientsFullName = "JMac";
            request.RecipientsEmailAddress = "jamesmacdonald@live.com.au";

            //request.RecipientsFullName = "Colin Byrne";
            //request.RecipientsEmailAddress = "ColinByrne@tboae.com"; 

            request.Subject = "This is a Test";
            request.FromName = "Colin Byrne";
            request.FromEmailAddress ="ColinByrne@tboae.com"; 
 
            var opStatus = _emailService.TestEmail(request);
        }
    }

}
