﻿using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObject
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string? KnownAs { get; set; }

        [Required]
        public string? Gender { get; set; }

        [Required]
        public DateOnly? DateOfBirth { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Country { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string password { get; set; } = string.Empty;
    }
}
