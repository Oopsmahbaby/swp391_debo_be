using swp391_debo_be.Attributes;
using System.ComponentModel.DataAnnotations;

namespace swp391_debo_be.Dto.Implement
{
    public class CreateUserDto
    {
        [ValidEmail]
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? password { get; set; }
    }
}
