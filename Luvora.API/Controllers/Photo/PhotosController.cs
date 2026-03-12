using System.Security.Claims;
using Luvora.Application.DTOs.Photo;
using Luvora.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luvora.API.Controllers.Photo
{
    [Route("api/photos")]
    [ApiController]
    [Authorize]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto([FromForm] UploadPhotoRequestDto request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var photoUrl = await _photoService.UploadPhotoAsync(userId, request.File);

            return Ok(new { photoUrl });
        }

        [HttpGet]
        public async Task<IActionResult> GetPhotos()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var photos = await _photoService.GetUserPhotosAsync(userId);

            return Ok( photos );
        }

        [HttpDelete("photoId")]
        public async Task<IActionResult> DeletePhoto(Guid photoId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _photoService.DeletePhotoAsync(userId, photoId);

            return Ok("Photo deleted.");
        }
    }
}
