namespace SocialMediaMediaDemo.Models
{
    public class MediaFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
