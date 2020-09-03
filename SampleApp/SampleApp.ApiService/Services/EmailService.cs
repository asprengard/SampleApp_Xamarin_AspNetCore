using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SampleApp.ApiService.Models;
using System;
using System.Threading.Tasks;

namespace SampleApp.ApiService.Services
{
    /// <summary>
    /// Sending Email
    /// http://www.mimekit.net/
    /// </summary>
    public class EmailService : IEmailService
    {
        EmailProviderAccount _emailAccount;

        public EmailService(IConfiguration configuration)
        {
            _emailAccount = new EmailProviderAccount();
            _emailAccount.EnableSSL = configuration.GetValue<bool>("EmailProviderAccount:EnableSSL");
            _emailAccount.Hostname = configuration.GetValue<string>("EmailProviderAccount:Host");
            _emailAccount.Password = configuration.GetValue<string>("EmailProviderAccount:Password");
            _emailAccount.Port = configuration.GetValue<int>("EmailProviderAccount:Port");
            _emailAccount.SenderDisplayname = configuration.GetValue<string>("EmailProviderAccount:SenderDisplayname");
            _emailAccount.SenderEmail = configuration.GetValue<string>("EmailProviderAccount:SenderEmail");
            _emailAccount.Username = configuration.GetValue<string>("EmailProviderAccount:Username");
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (_emailAccount == null)
            {
                return Task.FromResult(false);
            }

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailAccount.SenderDisplayname, _emailAccount.SenderEmail));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = htmlMessage
            };


            using (var client = new SmtpClient())
            {
                try
                {
                    if (_emailAccount.EnableSSL)
                    {
                        client.Connect(_emailAccount.Hostname, _emailAccount.Port,
                            MailKit.Security.SecureSocketOptions.Auto);
                    }
                    else
                    {
                        client.Capabilities &= ~SmtpCapabilities.Pipelining;
                        client.Connect(_emailAccount.Hostname, _emailAccount.Port,
                            MailKit.Security.SecureSocketOptions.None);
                    }

                    if (!string.IsNullOrEmpty(_emailAccount.Username) && !string.IsNullOrEmpty(_emailAccount.Password))
                        client.Authenticate(_emailAccount.Username, _emailAccount.Password);

                    client.Send(message);

                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    return Task.FromResult(ex);
                }
            }

            return Task.FromResult(true);
        }
    }
}
