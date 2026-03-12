using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Domain.Entities
{
    public class UserPhoto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string PhotoUrl { get; set; } = null!;

        public int DisplayOrder { get; set; }

        public bool IsPrimary { get; set; }

        public DateTime CreatedAt { get; set; }

        private UserPhoto() { }

        public UserPhoto(Guid userId, string photoUrl, int displayOrder, bool isPrimary)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            PhotoUrl = photoUrl;
            DisplayOrder = displayOrder;
            IsPrimary = isPrimary;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
