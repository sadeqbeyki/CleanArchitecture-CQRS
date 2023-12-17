using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Identity.Persistance.Identity;

public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "RequiredField")]
    [DisplayName("First Name")]
    [StringLength(40, MinimumLength = 2, ErrorMessage = "StringMinMaxLength")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "RequiredField")]
    [DisplayName("Last Name")]
    [StringLength(40, MinimumLength = 2, ErrorMessage = "StringMinMaxLength")]
    public string LastName { get; set; }


    #region Additional fields
    [Required(ErrorMessage = "RequiredField")]
    [DisplayName("Language/Culture")]
    public int Culture { get; set; }

    [Required(ErrorMessage = "RequiredField")]
    [DisplayName("Joined On")]
    public DateTime? JoinedOn { get; set; }
    #endregion
}
