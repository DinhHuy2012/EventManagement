using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Pages.Admin.Events
{
    public class DetailsModel : PageModel
    {
        private readonly Asm3EventManagementContext _context;
        private readonly IHubContext<SignalRHub> _hubContext;

        public DetailsModel(Asm3EventManagementContext context, IHubContext<SignalRHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public Event Event { get; set; } = default!;

        [BindProperty]
        public int TotalRegistered { get; set; }
        public List<EventCategory> EventCategories { get; set; } = new List<EventCategory>();
        public List<Attendee> Attendees { get; set; } = new List<Attendee>();

		public async Task<IActionResult> OnGetAsync(int? Id)
		{
			// If the Id is null or invalid, remain on the page
			if (!Id.HasValue)
			{
				return Page();
			}

			// Query the event based on the Id
			Event = await _context.Events.Include(x => x.Category)
										 .Include(x => x.Organizer)
										 .FirstOrDefaultAsync(m => m.EventId == Id);

			// If the event is not found, remain on the page
			if (Event == null)
			{
				// Optionally, you can add a message or flag to notify the user
				ModelState.AddModelError(string.Empty, "The event does not exist.");
				return Page();
			}

			// Fetch total registered attendees and list of attendees
			TotalRegistered = await _context.Attendees
											.Where(x => x.EventId == Id && x.Status == "Live")
											.CountAsync();
			Attendees = await _context.Attendees
									  .OrderBy(x => x.RegistrationTime)
									  .Include(x => x.Event)
									  .Include(x => x.User)
									  .Where(x => x.EventId == Id && x.Status == "Live")
									  .ToListAsync();

			// Return the page
			return Page();
		}



	}
}
