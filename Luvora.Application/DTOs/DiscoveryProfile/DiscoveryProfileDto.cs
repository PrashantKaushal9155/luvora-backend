using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luvora.Application.DTOs.DiscoveryProfile
{
    public class DiscoveryProfileDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string City { get; set; } = null!;
        public string? Bio {  get; set; }
        public string? Occupation { get; set; }
    }
}
