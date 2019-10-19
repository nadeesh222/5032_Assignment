using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace JHMS.Utils
{
    public class EmailSender
    {  // Please use your API KEY here.
        // Please use your API KEY here.
        private const String API_KEY = "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";

        public void SendSingleEmail(String toEmail, String subject, String contents)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("noreply@localhost.com", "Jetwing Hotels Booking Confirmation");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = client.SendEmailAsync(msg);
        }

        public void SendBulkEmail(String toEmailList, String subject, String contents)
        {
            List<EmailAddress> tolist = new List<EmailAddress>();

            String[] list = toEmailList.Split('|');
            foreach (String s in list)
            {
                tolist.Add(new EmailAddress(s, ""));
            }

            if (tolist.Count > 0)
            {
                var client = new SendGridClient(API_KEY);
                var from = new EmailAddress("noreply@localhost.com", "New offer for jetwing hotels");

                var plainTextContent = contents;
                var htmlContent = "<p>" + contents + "</p>";
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tolist, subject, plainTextContent, htmlContent);
               
                string filePath = HttpContext.Current.Request.MapPath("~/Utils/documents/details.pdf");


                var bytes = File.ReadAllBytes(filePath);
                var file = Convert.ToBase64String(bytes);
                msg.AddAttachment("details.pdf",file);
                var response = client.SendEmailAsync(msg);
            }
        }

    }
}