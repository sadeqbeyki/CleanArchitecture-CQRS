﻿using System.ComponentModel.DataAnnotations;

namespace Identity.Application.DTOs.Auth;

public class LoginUserDto
{
    //[Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
    //[EmailAddress(ErrorMessage = "InvalidMailId")]
    //[DataType(DataType.EmailAddress)]
    //[Display(Name = "Email Address")]
    //public string Email { get; set; }

    [Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
    [Display(Name = "User Name")]
    public string Username { get; set; }

    [Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
