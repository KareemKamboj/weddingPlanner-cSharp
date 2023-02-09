using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618
namespace WeddingPlanner.Models;

public class Wedding
{
    [Key]
    public int WeddingId { get; set; }

    [Required(ErrorMessage = "is required")]
    public string WedderOne { get; set; }

    [Required(ErrorMessage = "is required")]
    public string WedderTwo { get; set; }

    [Required(ErrorMessage = "is required")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "is required")]
    public int Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set;} = DateTime.Now;

    //foreign keys
    public int UserId { get; set; }
    public User? Planner { get; set; }
    
    List<Wedding> PlannedWeddings { get; set; } = new List<Wedding>();

    public List<Rsvp> WeddingRsvp { get; set; } = new List<Rsvp>();
}