using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models;

public partial class Event
{

    public int EventId { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Start time is required.")]
    [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "End time is required.")]
    [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
    [DateGreaterThan("StartTime", ErrorMessage = "End time must be after start time.")]
    public DateTime EndTime { get; set; }


    [Required(ErrorMessage = "Location is required.")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Description is required.")]

    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid Category ID.")]
    public int? CategoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid Organizer ID.")]
    public int? OrganizerId { get; set; }

    public int? Status { get; set; }

    public string? Image { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
    public int? Capacity { get; set; }



    public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();

    public virtual EventCategory? Category { get; set; }

    public virtual User? Organizer { get; set; }
}

