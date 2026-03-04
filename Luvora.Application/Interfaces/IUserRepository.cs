using System;
using System.Threading.Tasks;
using Luvora.Domain.Entities;

namespace Luvora.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync (Guid id);
        Task AddAsync (User user);
        Task<bool> UpdatePasswordAsync(Guid userId, string hashedPassword);
    }
}
