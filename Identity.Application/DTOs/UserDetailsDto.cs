using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Identity.Application.DTOs.Attributes;
using Identity.Application.Common.Enums;

namespace Identity.Application.DTOs;

public class UserDetailsDto
{
    #region Basic user details
    [Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
    [ValidateGuid]
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
    [DisplayName("User Name")]
    [StringLength(40, MinimumLength = 2, ErrorMessage = "StringMinMaxLength")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
    [DisplayName("First Name")]
    [StringLength(40, MinimumLength = 2, ErrorMessage = "StringMinMaxLength")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
    [DisplayName("Last Name")]
    [StringLength(40, MinimumLength = 2, ErrorMessage = "StringMinMaxLength")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
    [EmailAddress(ErrorMessage = "InvalidMailId")]
    [Display(Name = "Email Address")]
    [DataType(DataType.EmailAddress)]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "StringMinMaxLength")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
    [StringLength(20)]
    [Phone(ErrorMessage = "InvalidPhone")]
    [DisplayName("Phone Number")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = string.Empty;
    #endregion

    #region Additional Fields
    [DisplayName("Language/Culture")]
    public SupportedCulture Culture { get; set; }

    [Required(ErrorMessage = "RequiredField")]
    [DisplayName("Joined On")]
    public DateTime? JoinedOn { get; set; }

    [Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
    [DisplayName("Role")]
    public IList<string> Roles { get; set; } = new List<string>();  
    #endregion
}
