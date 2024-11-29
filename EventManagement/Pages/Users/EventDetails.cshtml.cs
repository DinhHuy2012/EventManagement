using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;

namespace EventManagement.Pages.Users
{
	public class EventDetailsModel : PageModel
	{
		private readonly Asm3EventManagementContext _context;
		private readonly IHubContext<SignalRHub> _hubContext;

		public EventDetailsModel(Asm3EventManagementContext context, IHubContext<SignalRHub> hubContext)
		{
			_context = context;
			_hubContext = hubContext;
		}
		[BindProperty]
		public Attendee Attendee { get; set; }

		public Event Event { get; set; } = default!;

		public int TotalRegistered { get; set; }

		public async Task<IActionResult> OnGetAsync(int? EventId)
		{
			var userID = HttpContext.Session.GetString("userID");
		
			if (!EventId.HasValue)
			{
				return Page();
			}
			// Query the event based on the Id
			Event = await _context.Events.Include(x => x.Category)
										 .Include(x => x.Organizer)
										 .FirstOrDefaultAsync(m => m.EventId == EventId);

			if (Event == null)
			{
				// Optionally, you can add a message or flag to notify the user
				ModelState.AddModelError(string.Empty, "The event does not exist.");
				return Page();
			}

			// Fetch total registered attendees and list of attendees
			TotalRegistered = await _context.Attendees
											.Where(x => x.EventId == EventId && x.Status == "Live")
											.CountAsync();

			await _hubContext.Clients.All.SendAsync("UpdateTotalRegistered", TotalRegistered);

			// Return the page
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? EventId)
		{
			var userID = HttpContext.Session.GetString("userID");

			if (EventId == null)
			{
				ModelState.AddModelError(string.Empty, "Event ID is required.");
				return Page();
			}
			var totalRegistered = await _context.Attendees
				.Where(x => x.EventId == EventId && x.Status == "Live")
				.CountAsync();

			// Fetch the event to check its capacity
			var eventDetail = await _context.Events
				.FirstOrDefaultAsync(e => e.EventId == EventId);

			if (eventDetail == null)
			{
				// Event does not exist
				ModelState.AddModelError(string.Empty, "The event does not exist.");
				return Page();
			}

			// Check if the event's capacity is reached
			if (totalRegistered >= eventDetail.Capacity)
			{
				// Event is full, return with an error
				TempData["ErrorMessage"] = "Event is already at full capacity.";
				return RedirectToPage("/Users/EventDetails", new { EventId });
			}



			_context.Attendees.Add(Attendee);
			await _context.SaveChangesAsync();

			// Notify all clients about the new event
			await _hubContext.Clients.All.SendAsync("LoadMyEvents");
			await _hubContext.Clients.All.SendAsync("UpdateTotalRegistered");

			// Add success message to TempData for feedback on redirection
			TempData["SuccessMessage"] = "Event registered successfully.";

			return RedirectToPage("/Users/EventDetails", new { EventId });
		}


	}
}
