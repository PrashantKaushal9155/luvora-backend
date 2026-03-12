using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Luvora.Application.DTOs.Photo
{
    public class UploadPhotoRequestDto
    {
        public IFormFile File { get; set; } = null!;
    }
}
