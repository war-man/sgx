﻿using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
