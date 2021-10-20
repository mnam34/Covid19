using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
namespace Common.Helpers
{
    public class SendEmail
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private static readonly string EmailServiceHost = "mail.gdt.gov.vn";
        private static readonly int EmailServicePort = 587;

        public static int SendMail2(string fromEmail, string fromName, string fromPassword, string toEmail, string subject, string body, string trainingMode)
        {
            if (trainingMode == "1")
            {
                subject = "[Thử nghiệm] " + subject;
            }
            var fromAddress = new MailAddress(fromEmail, fromName);
            string userName = fromAddress.Address;
            if (fromAddress.Address.EndsWith("gdt.gov.vn"))
            {
                var tmp = fromAddress.Address.Split('@')[0];
                userName = string.Format(@"mb\{0}", tmp);
            }

            var smtp = new SmtpClient
            {
                Host = EmailServiceHost,
                Port = EmailServicePort,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(userName, fromPassword)
            };
            if (fromAddress.Address.EndsWith("gdt.gov.vn"))
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
            }
            var message = new MailMessage()
            {
                From = fromAddress,
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            if (!string.IsNullOrEmpty(toEmail.Trim()))
            {
                foreach (string bccm in toEmail.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrEmpty(bccm.Trim()))
                    {
                        if (IsValidEmail(bccm.Trim()))
                        {
                            if (trainingMode == "1")
                            {
                                message.Bcc.Add(new MailAddress("mvnam.tho@gdt.gov.vn"));
                            }
                            else
                            {
                                message.Bcc.Add(new MailAddress(bccm.Trim()));
                            }                            
                        }
                    }
                }

            }
            try
            {
                smtp.Send(message);
                return 1;
            }
            catch//(Exception ex)
            {
                //throw ex;
                return 0;
            }
        }
    }
}
