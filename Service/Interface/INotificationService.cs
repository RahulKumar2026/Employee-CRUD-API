using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Enums;

namespace Employee_CRUD_API.Service.Interface
{
    public interface INotificationService
    {
        Task SendAsync(NotificationType notificationType, string recipient,string subject,string message, List<EmailAttachmentDto>? attachments = null);
    }
}
