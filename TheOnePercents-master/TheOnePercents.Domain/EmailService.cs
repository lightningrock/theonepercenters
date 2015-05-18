using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using SendGridMail;
using SendGridMail.Transport;
using System.Web;
using System.Web.Mvc;
using TheOnePercents.Repository;
using TheOnePercents.Model;
using TheOnePercents.Repository.Infrastructure;
using TheOnePercents.Domain.ViewModel.Email;


namespace TheOnePercents.Domain
{
    public class EmailService : IEmailService {

        private readonly IRepositoryBase<theonepercentersEntities1> _repositoryBase;

        public EmailService(IRepositoryBase<theonepercentersEntities1> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        private SendGrid CreateSendGridInstance(SendEmailRequestBase sendEmailRequestBase)
        {
            // Create the email object first, then add the properties
            SendGrid sendGridInstance = SendGrid.GetInstance();

            // Add the message properties
            sendGridInstance.From = new MailAddress(@"" +  sendEmailRequestBase.FromName + "<" +  sendEmailRequestBase.FromEmailAddress + ">");

            return sendGridInstance;
        }

        private bool SendEmail(SendGrid sendGridInstance)
        {

            // Create credentials
            var credentials = new NetworkCredential("azure_2fc477bcae2e6e5b2c6b68f0b33f79d7@azure.com", "Hq8LfRYu31MsZ4X");

            // Create an SMTP transport for sending email
            var transportSMTP = SMTP.GetInstance(credentials);

            // Send the email
            transportSMTP.Deliver(sendGridInstance);

            return true;

        }

        public OperationStatus TestEmail(SendEmailRequestBase sendEmailRequestBase)
        {
            OperationStatus opStatus = new OperationStatus() { Status = true };

            SendGrid sendGridInstance = CreateSendGridInstance(sendEmailRequestBase);

            string recipient = @"" + sendEmailRequestBase.RecipientsFullName + "<" + sendEmailRequestBase.RecipientsEmailAddress + ">";

            sendGridInstance.AddTo(recipient);

            var link = "www.tops.com";

            // Add the HTML and Text bodies
            sendGridInstance.Subject = "TOPS - Reset Password Request";
            sendGridInstance.Html = ActionResetPassword(link,
                "Click Here to Restset Password ",
                "TOPS",
                sendEmailRequestBase.RecipientsFullName);


            bool returnValue = SendEmail(sendGridInstance);

            return opStatus;
        }

        #region "Reset Password"
        public OperationStatus SendResetPasswordEmail(SendResetPasswordEmailRequest SendResetPasswordEmailRequest)
        {

            OperationStatus opStatus = new OperationStatus() { Status = true };

            SendEmailRequestBase sendEmailRequestBase = (SendEmailRequestBase)SendResetPasswordEmailRequest;

            Company company = _repositoryBase.FindNonTracking<Company>(x => x.CompanyID == SendResetPasswordEmailRequest.CompanyID);

            Setting setting = _repositoryBase.FindNonTracking<Setting>(x => x.SettingID == company.SettingID);

            if (sendEmailRequestBase.FromEmailAddress == null)
            {
                sendEmailRequestBase.FromEmailAddress = setting.CompanyEmail;
            }

            SendGrid sendGridInstance = CreateSendGridInstance(sendEmailRequestBase);

            string recipient = @"" + SendResetPasswordEmailRequest.RecipientsFullName + "<" + SendResetPasswordEmailRequest.RecipientsEmailAddress + ">";

            sendGridInstance.AddTo(recipient);

            sendGridInstance.Subject = SendResetPasswordEmailRequest.Subject;

            var Url = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
            var link = Url.Action("SetupPassword", "Account", new
            {
                @area = string.Empty,
                companyID = SendResetPasswordEmailRequest.CompanyID.ToString(),
                userID = SendResetPasswordEmailRequest.UserID.ToString(),
                passwordSetIdentifier = SendResetPasswordEmailRequest.PasswordSetIdentifier.ToString(),
                application = SendResetPasswordEmailRequest.Application
            }, "http");

            sendGridInstance.Html = ActionResetPassword(link,
                "Click Here to Reset your Password",
                company.CompanyName,
                SendResetPasswordEmailRequest.RecipientsFullName);

            bool returnValue = SendEmail(sendGridInstance);

            return opStatus;
        }


        /// <summary>
        /// Format the Reset Password email using incline styling.
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="message"></param>
        /// <param name="companyName"></param>
        /// <param name="recipientsFullName"></param>
        /// <returns></returns>
        public string ActionResetPassword(string URL, string message, string companyName, string recipientsFullName)
        {

            StringBuilder emailBuilder = new StringBuilder();
            emailBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; margin: 0; padding: 0;\">");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("<head>");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("<meta name=\"viewport\" content=\"width=device-width\" />");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("<title>Sign Waiver Email</title>");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("<style type=\"text/css\">");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("img {max-width: 100%;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("body {-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; width: 100% !important; height: 100%; line-height: 1.6;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("body {background-color: #f6f6f6;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("@media only screen and (max-width: 640px) {");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("h1 {font-weight: 600 !important; margin: 20px 0 5px !important;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("h2 {font-weight: 600 !important; margin: 20px 0 5px !important;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("h3 {font-weight: 600 !important; margin: 20px 0 5px !important;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("h4 {font-weight: 600 !important; margin: 20px 0 5px !important;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("h1 {font-size: 22px !important;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("h2 {font-size: 18px !important;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("h3 {font-size: 16px !important;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append(".container {width: 100% !important;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append(".content {padding: 10px !important;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append(".content-wrapper {padding: 10px !important;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append(".invoice {width: 100% !important;}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("}");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("</style>");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("</head>");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("<body style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; -webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; width: 100% !important; height: 100%; line-height: 1.6; background: #f6f6f6; margin: 0; padding: 0;\">");
            emailBuilder.Append(Environment.NewLine);


            emailBuilder.Append("<table class=\"body-wrap\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; width: 100%; background: #f6f6f6; margin: 0; padding: 0;\">");
            emailBuilder.Append("<tr style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; margin: 0; padding: 0;\">");
            emailBuilder.Append("<td style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0;\" valign=\"top\"></td>");
            emailBuilder.Append("<td class=\"container\" width=\"600\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; display: block !important; max-width: 600px !important; clear: both !important; margin: 0 auto; padding: 0;\" valign=\"top\">");
            emailBuilder.Append("<div class=\"content\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; max-width: 600px; display: block; margin: 0 auto; padding: 20px;\">");
            emailBuilder.Append("<table class=\"main\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; border-radius: 3px; background: #fff; margin: 0; padding: 0; border: 1px solid #e9e9e9;\">");
            emailBuilder.Append("<tr style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; margin: 0; padding: 0;\">");
            emailBuilder.Append("<td class=\"content-wrap\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 20px;\" valign=\"top\">");
            emailBuilder.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; margin: 0; padding: 0;\">");

            emailBuilder.Append("<tr style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; margin: 0; padding: 0;\">");
            emailBuilder.Append("<td class=\"content-block\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;\" valign=\"top\">");
            // Athlete Name 
            emailBuilder.Append("Hey ");
            emailBuilder.Append(recipientsFullName);
            emailBuilder.Append(",");
            emailBuilder.Append("</td>");
            emailBuilder.Append("</tr>");

            emailBuilder.Append("<tr style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; margin: 0; padding: 0;\">");
            emailBuilder.Append("<td class=\"content-block\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;\" valign=\"top\">");
            // Message 
            emailBuilder.Append("A request has been received to reset your ");
            emailBuilder.Append(companyName);
            emailBuilder.Append(" password.");
            emailBuilder.Append("</td>");
            emailBuilder.Append("</tr>");


            emailBuilder.Append("<tr style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; margin: 0; padding: 0;\">");
            emailBuilder.Append("<td class=\"content-block\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;\" valign=\"top\">");
            // Instructions
            emailBuilder.Append("If this came from you please click on the link below to reset your password.");
            emailBuilder.Append("</td>");
            emailBuilder.Append("</tr>");
            emailBuilder.Append("<tr style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; margin: 0; padding: 0;\">");
            emailBuilder.Append("<td class=\"content-block\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;\" valign=\"top\">");

            // Button Link and thank you. 
            emailBuilder.Append("<a href=\"");
            emailBuilder.Append(URL);
            emailBuilder.Append(" class=\"btn-primary\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; color: #FFF; text-decoration: none; line-height: 2; font-weight: bold; text-align: center; cursor: pointer; display: inline-block; border-radius: 5px; text-transform: capitalize; background: #348eda; margin: 0; padding: 0; border-color: #348eda; border-style: solid; border-width: 10px 20px;\">Click To Reset...</a>");
            emailBuilder.Append("</td>");
            emailBuilder.Append("</tr>");
            emailBuilder.Append("<tr style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; margin: 0; padding: 0;\">");
            emailBuilder.Append("<td class=\"content-block\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;\" valign=\"top\">");
            emailBuilder.Append("Thanks ");
            emailBuilder.Append("&mdash; ");
            emailBuilder.Append(companyName);
            emailBuilder.Append("</td>");
            emailBuilder.Append("</tr>");
            emailBuilder.Append("</table>");
            emailBuilder.Append("</td>");
            emailBuilder.Append("</tr>");
            emailBuilder.Append("</table>");
            emailBuilder.Append("<div class=\"footer\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; width: 100%; clear: both; color: #999; margin: 0; padding: 20px;\">");
            emailBuilder.Append("<table width=\"100%\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; margin: 0; padding: 0;\">");
            emailBuilder.Append("<tr style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; margin: 0; padding: 0;\">");
            emailBuilder.Append("<td class=\"aligncenter content-block\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 12px; vertical-align: top; text-align: center; margin: 0; padding: 0 0 20px;\" align=\"center\" valign=\"top\">");
            emailBuilder.Append("<a href=\"http://tboae.com\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 12px; color: #999; text-decoration: underline; margin: 0; padding: 0;\">{tboae}\\\\</a></td></td>");
            emailBuilder.Append("</tr>");
            emailBuilder.Append("</table>");
            emailBuilder.Append("</div></div>");
            emailBuilder.Append("</td>");
            emailBuilder.Append("<td style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0;\" valign=\"top\"></td>");
            emailBuilder.Append("</tr>");
            emailBuilder.Append("</table>");
            emailBuilder.Append("</body>");
            emailBuilder.Append(Environment.NewLine);
            emailBuilder.Append("</html>");

            return emailBuilder.ToString();
        }

    #endregion

        public string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (!string.IsNullOrWhiteSpace(appUrl)) appUrl += "/";

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }
    }

    public interface IEmailService {
        OperationStatus TestEmail(SendEmailRequestBase sendEmailRequestBase);
        OperationStatus SendResetPasswordEmail(SendResetPasswordEmailRequest SendResetPasswordEmailRequest);        
    }
}
