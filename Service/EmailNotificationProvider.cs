using Employee_CRUD_API.Enums;
using Employee_CRUD_API.Service.Interface;

namespace Employee_CRUD_API.Service
{
    public class EmailNotificationProvider : INotificationProvider
    {
        private readonly ILogger<EmailNotificationProvider> _logger;

        public EmailNotificationProvider(ILogger<EmailNotificationProvider> logger)
        {
            _logger = logger;
        }
        public NotificationType Type => NotificationType.Email;
        public async Task SendAsync( string recipient,string subject,string message)
        {
            try
            {
                _logger.LogInformation("Sending EMAIL notification");

                _logger.LogInformation("Recipient: {Recipient}",recipient);

                _logger.LogInformation("Subject: {Subject}",subject);

                _logger.LogInformation("Message: {Message}",message);

                // Actual SMTP / Graph / SendGrid
                // implementation will come here.

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
