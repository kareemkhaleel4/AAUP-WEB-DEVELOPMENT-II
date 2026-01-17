using Microsoft.AspNetCore.Http;

namespace LinkedInLite.Api.Services.FileStorage;

public interface IFileStorage
{
    Task<string> SavePostImageAsync(IFormFile file);
}
