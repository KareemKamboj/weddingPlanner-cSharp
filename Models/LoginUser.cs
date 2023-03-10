#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models;

[NotMapped]
public class LoginUser
{

    [Required(ErrorMessage = "is required")]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    public string LoginEmail { get; set; }

    [Required(ErrorMessage = "is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string LoginPassword { get; set; }
}