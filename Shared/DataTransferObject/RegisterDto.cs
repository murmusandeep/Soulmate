using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObject
{
    public class RegisterDto
    {
        [Required]
        public string username { get; set; } = string.Empty;

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string password { get; set; } = string.Empty;
    }
}
