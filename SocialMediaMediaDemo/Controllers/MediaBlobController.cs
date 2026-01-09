using Microsoft.AspNetCore.Mvc;
using SocialMediaMediaDemo.Data;
using SocialMediaMediaDemo.Models;

namespace SocialMediaMediaDemo.Controllers
{
    [ApiController]
    [Route("api/blob-media")]
    public class MediaBlobController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MediaBlobController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest();

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var media = new MediaBlob
            {
                Data = ms.ToArray(),
                ContentType = file.ContentType,
                FileName = file.FileName,
                UploadedAt = DateTime.UtcNow
            };

            _context.MediaBlobs.Add(media);
            await _context.SaveChangesAsync();

            return Ok(new { media.Id });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var media = _context.MediaBlobs.Find(id);
            if (media == null)
                return NotFound();

            return File(media.Data, media.ContentType);
        }
    }
}
