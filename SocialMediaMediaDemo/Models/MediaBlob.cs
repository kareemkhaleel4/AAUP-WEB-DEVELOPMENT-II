namespace SocialMediaMediaDemo.Models
{
    public class MediaBlob
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}