using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Luvora.Application.Interfaces.Services
{
    public interface IPhotoService
    {
        Task<string> UploadPhotoAsync(Guid userId, IFormFile file);
        Task<IEnumerable<UserPhoto>> GetUserPhotosAsync(Guid userId);
        Task DeletePhotoAsync(Guid userId, Guid photoId);
    }
}
