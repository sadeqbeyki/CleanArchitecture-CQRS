using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace Identity.Persistance.Identity;

public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "RequiredField")]
    [DisplayName("First Name")]
    [StringLength(40, MinimumLength = 2, ErrorMessage = "StringMinMaxLength")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "RequiredField")]
    [DisplayName("Last Name")]
    [StringLength(40, MinimumLength = 2, ErrorMessage = "StringMinMaxLength")]
    public string LastName { get; set; } = String.Empty;


    #region Additional fields
    [Required(ErrorMessage = "RequiredField")]
    [DisplayName("Language/Culture")]
    public int Culture { get; set; }

    [Required(ErrorMessage = "RequiredField")]
    [DisplayName("Joined On")]
    public DateTime? JoinedOn { get; set; }
    #endregion
}
