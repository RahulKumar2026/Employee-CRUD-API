namespace Employee_CRUD_API.DTOs
{
    public class EmailAttachmentDto
    {
        public string FileName { get; set; } = string.Empty;
        public byte[] FileData { get; set; } = Array.Empty<byte>(); 
        public string ContentType { get; set; } = string.Empty; 
    }
}
