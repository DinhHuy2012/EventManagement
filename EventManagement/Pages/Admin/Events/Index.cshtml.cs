using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EventManagement.Pages.Admin.Events
{
    public class IndexModel : PageModel
    {
        private readonly Asm3EventManagementContext _context;
        private readonly IHubContext<SignalRHub> _hubContext;

        public IndexModel(Asm3EventManagementContext context, IHubContext<SignalRHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public IList<Event> Events { get; set; } = default!;


		public async Task OnGetAsync()
		{
			Events = await _context.Events.ToListAsync();

		}
		[BindProperty(SupportsGet = true)]
		public string SearchName { get; set; } = string.Empty;

		public ContentResult OnGetGetEventsAsync(string search, string sortOption, int pageNumber = 1, int pageSize = 5)
		{
			var query = string.IsNullOrEmpty(search)
				? _context.Events
				: _context.Events
					.Where(u => u.Title.Contains(search)
							 || u.Category.CategoryName.Contains(search));

			// Apply sorting based on the selected sortOption
			switch (sortOption)
			{
				case "newest":
					query = query.OrderByDescending(u => u.EventId);
					break;
				case "oldest":
					query = query.OrderBy(u => u.EventId);
					break;
				case "accepted":
					query = query.Where(e => e.Status == 1); // Assuming status 1 is accepted
					break;
				case "pending":
					query = query.Where(e => e.Status == 2); // Assuming status 2 is rejected
					break;
				default:
					query = query.OrderByDescending(u => u.EventId); // Default to newest if no option is selected
					break;
			}

			var totalEvents = query.Count(); // Get total count of events matching the search criteria
			var totalEventsLive = _context.Events.Count(x => x.Status == 1); // Get total live events
			var totalEventsPending = _context.Events.Count(x => x.Status == 2); // Get total pending events

			var events = query
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.Select(e => new
				{
					e.EventId,
					e.Title,
					e.Image,
					e.StartTime,
					e.EndTime,
					e.Capacity,
					e.Location,
					Category = new { e.Category.CategoryId, e.Category.CategoryName },
					User = new { e.Organizer.UserId, e.Organizer.Username, e.Organizer.Avatar }, // Adjust according to your User model properties
					e.Status // Assuming status is a property of Event
				})
				.ToList();

			// Construct the result object to include both events and total count
			var result = new
			{
				Events = events,
				TotalEvents = totalEvents,
				TotalEventsLive = totalEventsLive,
				TotalEventsPending = totalEventsPending
			};

			string jsonStr = JsonSerializer.Serialize(result);
			return Content(jsonStr);
		}

        public async Task<IActionResult> OnGetApprove(int eventId)
        {
            var eventToUpdate = await _context.Events.FindAsync(eventId);
            if (eventToUpdate == null)
            {
                return NotFound();
            }

            eventToUpdate.Status = 1; // Ví dụ: 1 có thể là trạng thái "Approved"

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("LoadEvents");

            return RedirectToPage(); // Redirect back to the same page
        }

        public async Task<IActionResult> OnGetReject(int eventId)
        {
            var eventToUpdate = await _context.Events.FindAsync(eventId);
            if (eventToUpdate == null)
            {
                return NotFound();
            }

            eventToUpdate.Status = 3; // Ví dụ: 3 có thể là trạng thái "Rejected"

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("LoadEvents");

            return RedirectToPage(); // Redirect back to the same page
        }
    }
}
