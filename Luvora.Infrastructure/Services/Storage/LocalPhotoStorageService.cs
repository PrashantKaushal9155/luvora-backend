using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Luvora.Application.Interfaces.Services;
using Microsoft.Extensions.Hosting;

namespace Luvora.Infrastructure.Services.Storage
{
    public class LocalPhotoStorageService : IPhotoStorageService
    {
        private readonly IHostEnvironment _environment;

        public LocalPhotoStorageService(IHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SavePhotoAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            return $"/uploads/{fileName}";
        }

        public Task DeletePhotoAsync(string photoUrl)
        {
            var filePath = Path.Combine(_environment.ContentRootPath, photoUrl.TrimStart('/'));

            if(File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }
    }
}
