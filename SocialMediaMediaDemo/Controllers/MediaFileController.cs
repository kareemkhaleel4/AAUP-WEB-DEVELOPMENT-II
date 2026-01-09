using Microsoft.AspNetCore.Mvc;
using SocialMediaMediaDemo.Data;
using SocialMediaMediaDemo.Models;

namespace SocialMediaMediaDemo.Controllers
{
    [ApiController]
    [Route("api/file-media")]
    public class MediaFileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MediaFileController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest();

            var uploadsFolder = Path.Combine("wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(uploadsFolder, uniqueName);

            using var fs = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fs);

            var media = new MediaFile
            {
                FileName = file.FileName,
                FilePath = "/uploads/" + uniqueName,
                ContentType = file.ContentType,
                Size = file.Length,
                UploadedAt = DateTime.UtcNow
            };

            _context.MediaFiles.Add(media);
            await _context.SaveChangesAsync();

            return Ok(media);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var media = _context.MediaFiles.Find(id);
            if (media == null)
                return NotFound();

            return Redirect(media.FilePath);
        }
    }
}
