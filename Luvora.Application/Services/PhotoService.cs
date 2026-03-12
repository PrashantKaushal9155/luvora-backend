using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Application.Interfaces.Repositories;
using Luvora.Application.Interfaces.Services;
using Luvora.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Luvora.Application.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IUserPhotoRepository _userPhotoRepository;
        private readonly IPhotoStorageService _photoStorageService;

        public PhotoService(IUserPhotoRepository userPhotoRepository, IPhotoStorageService photoStorageService)
        {
            _userPhotoRepository = userPhotoRepository;
            _photoStorageService = photoStorageService;
        }

        public async Task<string> UploadPhotoAsync(Guid userId, IFormFile file)
        {
            // Max 6 photos rule
            var photoCount = await _userPhotoRepository.GetPhotoCountAsync(userId);

            if (photoCount >= 6)
                throw new Exception("Maximum 6 photos allowed.");

            // Save file locally
            var photoUrl = await _photoStorageService.SavePhotoAsync(file);

            var isPrimary = photoCount == 0;

            var photo = new UserPhoto(
                userId,
                photoUrl,
                photoCount + 1,
                isPrimary
            );

            await _userPhotoRepository.AddPhotoAsync(photo);

            return photoUrl;
        }

        public async Task<IEnumerable<UserPhoto>> GetUserPhotosAsync(Guid userId)
        {
            return await _userPhotoRepository.GetPhotosByUserIdAsync(userId);
        }
        public async Task DeletePhotoAsync(Guid userId, Guid photoId)
        {
            var photos = await _userPhotoRepository.GetPhotosByUserIdAsync(userId);

            var photo = photos.FirstOrDefault(p => p.Id == photoId) ?? throw new Exception("Photo not found.");

            await _photoStorageService.DeletePhotoAsync(photo.PhotoUrl);

            await _userPhotoRepository.DeletePhotoAsync(photoId, userId);
        }
    }
}
