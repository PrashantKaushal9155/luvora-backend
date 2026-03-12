using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Luvora.Application.Interfaces.Services
{
    public interface IPhotoStorageService
    {
        Task<string> SavePhotoAsync(IFormFile file);
        Task DeletePhotoAsync(string photoUrl);
    }
}
