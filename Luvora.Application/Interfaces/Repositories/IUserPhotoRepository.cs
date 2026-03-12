using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Domain.Entities;

namespace Luvora.Application.Interfaces.Repositories
{
    public interface IUserPhotoRepository
    {
        Task AddPhotoAsync(UserPhoto photo);
        Task<IEnumerable<UserPhoto>> GetPhotosByUserIdAsync(Guid userId);
        Task DeletePhotoAsync(Guid photoId, Guid userId);
        Task<int> GetPhotoCountAsync(Guid userId);
    }
}
