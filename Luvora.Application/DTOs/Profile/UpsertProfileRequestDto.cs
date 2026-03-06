using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luvora.Domain.Enums;

namespace Luvora.Application.DTOs.Profile
{
    public class UpsertProfileRequestDto
    {
        public string Name { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Gender PreferredGender { get; set; }
        public int MinPreferredAge { get; set; }
        public int MaxPreferredAge { get; set; }
        public string Bio { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Occupation { get; set; } = null!;
        public RelationshipStatus RelationshipStatus { get; set; }
    }
}
