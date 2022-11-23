using MediatR;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;

namespace ArtGallery.Application.Common.Helpers;

public class FileHelper
{
    public static async Task<string> UploadFile(IFormFile upLoad, string fileName, string filePath = null)
    {
        if (upLoad == null || upLoad.Length == 0)
        {
            return null;
        }
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        filePath = Path.Combine(filePath, fileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await upLoad.CopyToAsync(fileStream);
        }

        return filePath;
    }

}
