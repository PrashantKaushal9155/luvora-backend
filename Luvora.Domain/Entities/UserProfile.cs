using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Luvora.Domain.Enums;

namespace Luvora.Domain.Entities
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Gender PreferredGender { get; set; }
        public int MinPreferredAge { get; set; }
        public int MaxPreferredAge { get; set; }
        public string? Bio { get; set; }
        public string City { get; set; } = null!;
        public string? Country { get; set; }
        public string? Occupation { get; set; }
        public RelationshipStatus RelationshipStatus { get; set; }
        public bool ProfileCompleted { get; set; }
        public bool IsVisible { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastActiveAt { get; set; }
        public DateTime CreatedAt { get; set; }

        private UserProfile() { } // Required for Dapper

        public UserProfile(
            Guid userId,
            string name,
            DateTime dateOfBirth,
            Gender gender,
            Gender preferredGender,
            RelationshipStatus relationshipStatus,
            int minPreferredAge,
            int maxPreferredAge,
            string city,
            string? bio = null,
            string? country = null,
            string? occupation = null)
        {
            Id = Guid.NewGuid();
            UserId = userId;

            Name = name;
            DateOfBirth = dateOfBirth;

            Gender = gender;
            PreferredGender = preferredGender;

            MinPreferredAge = minPreferredAge;
            MaxPreferredAge = maxPreferredAge;

            City = city;
            Bio = bio;
            Country = country;
            Occupation = occupation;
            RelationshipStatus = relationshipStatus;

            ProfileCompleted = true; // since mandatory fields validated before calling constructor
            IsVisible = true;
            IsVerified = false;
            IsDeleted = false;

            CreatedAt = DateTime.UtcNow;
        }
    }
}
