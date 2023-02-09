#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models;
public class Rsvp
{

    [Key]

    public int RsvpId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdateddAt { get; set; } = DateTime.Now;

    //foregin keys
    public int UserId { get; set; }
    public User? User { get; set; }
    public int WeddingId { get; set; }
    public Wedding? Wedding { get; set; }
}