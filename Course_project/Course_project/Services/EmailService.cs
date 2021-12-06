using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Services
{
    /// <summary>
    /// Service for sending emails
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="subject">Subject</param>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("iTransition course project", "itransition-courseproject232@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 465, true);
                await client.AuthenticateAsync("itransition-courseproject232@yandex.ru", "omprnvltifoheetx");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
