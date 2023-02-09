#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models;
public class User
{

    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage =  "is required")]
    public string FirstName { get; set; }

    [Required(ErrorMessage =  "is required")]
    public string LastName { get; set; }

    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage =  "is required")]
    public string Email { get; set; }

    [Required(ErrorMessage =  "is required")]
    [MinLength(8, ErrorMessage = "Must be at least 8 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [NotMapped]
    [Required(ErrorMessage =  "is required")]
    [Compare("Password", ErrorMessage = "Doesn't match password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    List<Wedding> PlannedWeddings { get; set; } = new List<Wedding>();

    public List<Rsvp> UserRsvp { get; set; } = new List<Rsvp>();
}