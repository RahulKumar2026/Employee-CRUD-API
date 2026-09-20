using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Enums;
using Employee_CRUD_API.Service.Interface;

namespace Employee_CRUD_API.Service
{
    public class WhatsAppNotificationProvider : INotificationProvider
    {
        private readonly ILogger<WhatsAppNotificationProvider> _logger;

        public WhatsAppNotificationProvider(ILogger<WhatsAppNotificationProvider> logger)
        {
            _logger = logger;
        }
        public NotificationType Type => NotificationType.WhatsApp;
        public async Task SendAsync(string recipient,string subject,string message, List<EmailAttachmentDto>? attachments = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(recipient))
                    throw new ArgumentException("WhatsApp number is required.",nameof(recipient));

                _logger.LogInformation("Sending WhatsApp notification");

                _logger.LogInformation("WhatsApp Number: {Recipient}",recipient);

                _logger.LogInformation("Message: {Message}",message);

                // Later you will call a real WhatsApp API here.

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while sending WhatsApp notification");

                throw;
            }
        }
    }
}