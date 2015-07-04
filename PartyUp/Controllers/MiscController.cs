using PartyUp.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace PartyUp.Controllers
{
    public class MiscController : ApiController
    {
        [HttpPost]
        [Route("api/misc/sendfeedback")]
        public void SendFeedback(FeedbackDTO feedback)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential("gentry.riggen@gmail.com", "DontGetH4cked!");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;


            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("noreply@partyup.io", "Party Up");
            mail.To.Add(new MailAddress("gentryriggen@outlook.com"));
            mail.Subject = "Party Up Feedback";
            mail.IsBodyHtml = true;
            mail.Body = "<ul>" +
                            "<li><strong>FROM: </strong>" + feedback.Name + "</li>" +
                            "<li><strong>EMAIL: </strong>" + feedback.Email + "</li>" +
                            "<li><strong>Message: </strong>" + feedback.Message + "</li>" +
                        "</ul>";

            smtpClient.Send(mail);
        }
    }
}
