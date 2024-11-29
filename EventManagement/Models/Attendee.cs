using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models;

public partial class Attendee
{
    public int AttendeeId { get; set; }

    public int? UserId { get; set; }

    public int EventId { get; set; }

	[Required(ErrorMessage = "Email là bắt buộc.")]
	[EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
	public string Email { get; set; } = null!;

	[Required(ErrorMessage = "Tên là bắt buộc.")]
	[StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
	public string Name { get; set; } = null!;


	public DateTime RegistrationTime { get; set; }

    public string Status { get; set; } = null!;

    public virtual Event? Event { get; set; }

    public virtual User? User { get; set; }
}
