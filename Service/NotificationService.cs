using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Enums;
using Employee_CRUD_API.Service.Interface;

namespace Employee_CRUD_API.Service
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IEnumerable<INotificationProvider> _providers;

        public NotificationService(ILogger<NotificationService> logger,IEnumerable<INotificationProvider> providers)
        {
            _logger = logger;
            _providers = providers;
        }

        public async Task SendAsync(NotificationType notificationType,string recipient,string subject,string message, List<EmailAttachmentDto>? attachments = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(recipient))
                    throw new ArgumentException("Recipient is required.",nameof(recipient));

                var provider = _providers.FirstOrDefault(x => x.Type == notificationType);

                if (provider == null)
                {
                    throw new InvalidOperationException($"Notification provider not found for {notificationType}");
                }

                _logger.LogInformation("Sending {NotificationType} notification", notificationType);

                await provider.SendAsync(recipient,subject,message, attachments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while sending {NotificationType} notification",notificationType);

                throw;
            }
        }
    }
}