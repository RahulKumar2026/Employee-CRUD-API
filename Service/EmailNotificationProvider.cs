using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Enums;
using Employee_CRUD_API.Service.Interface;
using Employee_CRUD_API.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;



namespace Employee_CRUD_API.Service
{
    public class EmailNotificationProvider : INotificationProvider
    {
        private readonly ILogger<EmailNotificationProvider> _logger;
        private readonly SmtpSettings _smtpSetting;
        public EmailNotificationProvider(ILogger<EmailNotificationProvider> logger, SmtpSettings smtpSetting)
        {
            _logger = logger;
            _smtpSetting = smtpSetting;
        }
        public NotificationType Type => NotificationType.Email;
        public async Task SendAsync( string recipient,string subject,string message, List<EmailAttachmentDto>? attachments = null)
        {
            try
            {
                _logger.LogInformation("Sending EMAIL notification");

                _logger.LogInformation("Recipient: {Recipient}",recipient);

                _logger.LogInformation("Subject: {Subject}",subject);

                _logger.LogInformation("Message: {Message}",message);

                var buildmessage = new MimeMessage();

                //Sender
                buildmessage.From.Add(new MailboxAddress(_smtpSetting.FromName, _smtpSetting.FromEmail));

                // Recevire
                buildmessage.To.Add(MailboxAddress.Parse(recipient));

                //subject 
                buildmessage.Subject = subject;
                //body
                var bodyBuilder = new BodyBuilder
                {
                    TextBody = message
                };
                // Attachment
                if (attachments != null && attachments.Any()) 
                {
                    foreach (var at in attachments) 
                    {
                        if (at.FileData == null || at.FileData.Length == 0) 
                        { 
                            continue; 
                        }
                        bodyBuilder.Attachments.Add(at.FileName, at.FileData, ContentType.Parse(at.ContentType));
                    }
                }
                buildmessage.Body = bodyBuilder.ToMessageBody();

                //SMTP Client
                using var smptClient = new SmtpClient();

                await smptClient.ConnectAsync(_smtpSetting.Host, _smtpSetting.Port,SecureSocketOptions.StartTls);
                await smptClient.AuthenticateAsync(_smtpSetting.Username,_smtpSetting.Password);

                await smptClient.SendAsync(buildmessage);
                await smptClient.DisconnectAsync(true);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while sending email notification");

                throw;
            }
        }
    }
}
