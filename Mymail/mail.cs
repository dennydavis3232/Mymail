using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.IO;

namespace Mymail
{
    class mail
    {
        public void SendMail(string mail, string body, string subject, string destinationpath, string fileName)
        {
            var message = new MailMessage("dennyad036@gmail.com", mail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Set to true if you're sending HTML content

            };
            if (!string.IsNullOrEmpty(destinationpath))
            {
                try
                {
                    string attachmentFilePath = Path.Combine(destinationpath, fileName);
                    if (File.Exists(attachmentFilePath))
                    {
                        Attachment attachment = new Attachment(attachmentFilePath);
                        message.Attachments.Add(attachment);
                    }
                    else
                    {
                        Console.WriteLine("Attachment file not found: " + attachmentFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error adding attachment: " + ex.Message);
                }
            }


            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Credentials = new NetworkCredential("dennyad036@gmail.com", "ebsx gpdb xnph tevh");
                smtpClient.EnableSsl = true;
                smtpClient.Port = 587; // Set the SMTP port (usually 587 for TLS/STARTTLS)
                smtpClient.Send(message);

            }
        }

        public void SomeMethod(string body, string subject, string mail, string destinationpath, string fileName)
        {
            SendMail(mail, body, subject, destinationpath, fileName);
        }
    }


}

