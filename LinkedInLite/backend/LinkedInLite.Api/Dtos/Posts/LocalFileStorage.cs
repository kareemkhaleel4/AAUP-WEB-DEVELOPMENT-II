using Microsoft.AspNetCore.Http;

namespace LinkedInLite.Api.Services.FileStorage;

public class LocalFileStorage : IFileStorage
{
    private readonly IWebHostEnvironment _env;

    public LocalFileStorage(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SavePostImageAsync(IFormFile file)
    {
        var uploadsPath = Path.Combine(
            _env.WebRootPath,
            "uploads",
            "posts");

        Directory.CreateDirectory(uploadsPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var fullPath = Path.Combine(uploadsPath, fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        // public URL
        return $"/uploads/posts/{fileName}";
    }
}
