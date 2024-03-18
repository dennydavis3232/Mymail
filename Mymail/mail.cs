using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace Mymail
{
    class mail
    {
        public void SendMail(string[] emailAddressesArray, string body, string subject, string destinationpath, string fileName)
        {
            foreach (var mail in emailAddressesArray)
            {
                var message = new MailMessage(ConfigurationManager.AppSettings["SenderEmail"], mail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
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

                using (var smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SmtpHost"]))
                {
                    smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SmtpUsername"], ConfigurationManager.AppSettings["SmtpPassword"]);
                    smtpClient.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
                    smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                    smtpClient.Send(message);
                }
            }
        }


    }
}

        
    

