using System;
using System.Collections.Generic;

namespace EventManagement.Models;
using System.ComponentModel.DataAnnotations;

public partial class User
{
    public int UserId { get; set; }


    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
    public string Username { get; set; } = null!;



    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Password must be at least 4 characters long.")]
    public string Password { get; set; } = null!;


	[StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]

	public string? Fullname { get; set; }


	[Required(ErrorMessage = "Email is required.")]
	[EmailAddress(ErrorMessage = "Invalid Email Address.")]
	[StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
	public string Email { get; set; } = null!;

	[RegularExpression(@"^(\+[0-9]{1,3})?([0-9]{10})$",
	ErrorMessage = "Phone number is not valid. It should be a 10-digit number or include a country code.")]
	public string? Phone { get; set; }


	public string Role { get; set; } = null!;

    public string? Avatar { get; set; }

    public bool? Status { get; set; }

    public DateTime JoinDate { get; set; }

    public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
