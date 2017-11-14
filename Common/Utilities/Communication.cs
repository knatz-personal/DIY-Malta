using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Common.Utilities
{
    public class Communication
    {
        public void SendEmail(string emailToSend, string subject, AlternateView html)
        {
            var mm = new MailMessage();
            mm.To.Add(emailToSend);
            mm.From = new MailAddress("idiposable@gmail.com");
            mm.CC.Add("nathan.zwelibanzi.khupe@mcast.edu.mt");
            mm.Subject = subject;
            mm.AlternateViews.Add(html);
            mm.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("idiposable@gmail.com", "z0aQ&$xS!w1")
            };

            try
            {
                client.Send(mm);
            }
            catch (Exception)
            {
                throw new Exception("Error sending email");
            }
        }
    }
}