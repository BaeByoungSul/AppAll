﻿using System.ComponentModel.DataAnnotations;

namespace Models.Auth;

public class UserRegister
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters, dude!")]
    public string Password { get; set; } = string.Empty;
        
    [Required, Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }

    public string VerifyUserUrl { get; set  ;} = string.Empty;
}
    

