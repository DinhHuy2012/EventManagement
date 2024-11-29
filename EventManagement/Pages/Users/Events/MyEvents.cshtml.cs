using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Drawing.Printing;
using System.Linq;
using System.Text.Json;

namespace EventManagement.Pages.Users.Events
{
    public class MyEventsModel : PageModel
    {

        private readonly Asm3EventManagementContext _context;
        private readonly IHubContext<SignalRHub> _hubContext;

        public MyEventsModel(Asm3EventManagementContext context, IHubContext<SignalRHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        [BindProperty]
        public User Users { get; set; }

        public IList<Event> Events { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; } = string.Empty;
        public async Task<IActionResult> OnGetAsync(int Id)
        {
            // Check if the session is null
            if (HttpContext.Session.GetString("userID") == null)
            {
                // Redirect to the login page if the session is null
                return RedirectToPage("/Users/Login");
            }
            var userID = HttpContext.Session.GetString("userID");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == int.Parse(userID));// Lấy thông tin user.

            if (Id == null || userID == null)
            {
                return RedirectToPage("/Users/Error404");
            }
            Events = await _context.Events.Include(x => x.Category).Where(x => x.OrganizerId == user.UserId).ToListAsync();


            Users = user;

            return Page();
        }
        public ContentResult OnGetGetMyEventsAsync(string search, int pageNumber = 1, int pageSize = 5)
        {
            var userID = HttpContext.Session.GetString("userID");
            var query = string.IsNullOrEmpty(search)
                ? _context.Events.Where(x => x.OrganizerId == int.Parse(userID))
                : _context.Events
                    .Where(u => (u.Title.Contains(search) || u.Category.CategoryName.Contains(search))
                                && u.OrganizerId == int.Parse(userID));

            var eventIds = query.AsEnumerable().Select(e => e.EventId).ToList();

            var totalEvents = query.Count(); // Get total count of events matching the search criteria
            var totalAttendeesByEvent = _context.Attendees
      .Where(a => eventIds.Contains(a.EventId) && a.Status == "Live")
      .GroupBy(a => a.EventId)
      .Select(group => new
      {
          EventId = group.Key,
          TotalAttendees = group.Count()
      })
      .ToList();

            var totalRequestByEvent = _context.Attendees
.Where(a => eventIds.Contains(a.EventId) && a.Status == "Pending")
.GroupBy(a => a.EventId)
.Select(group => new
{
    EventId = group.Key,
    TotalRequest = group.Count()
});
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
                    e.OrganizerId,
                    e.Location,
                    Category = new { e.Category.CategoryId, e.Category.CategoryName },
                    User = new { e.Organizer.UserId, e.Organizer.Username, e.Organizer.Avatar }, // Adjust according to your User model properties
                    e.Status,


                })
                .ToList();

            // Construct the result object to include both events and total count
            var result = new
            {
                Events = events,
                TotalEvents = totalEvents,
                TotalAttendees = totalAttendeesByEvent,
                TotalRequest = totalRequestByEvent

            };

            string jsonStr = JsonSerializer.Serialize(result);
            return Content(jsonStr);
        }
        public async Task<IActionResult> OnGetDeleteEvent(int EventId)
        {
            // Fetch all attendees related to the event
            var attendeesToDelete = await _context.Attendees
                .Where(a => a.EventId == EventId)
                .ToListAsync();

            // Fetch the event to delete
            var eventToDelete = await _context.Events.FindAsync(EventId);

            // If the event is not found, return NotFound
            if (eventToDelete == null)
            {
                return NotFound();
            }

            // Remove all related attendees
            _context.Attendees.RemoveRange(attendeesToDelete);

            // Remove the event itself
            _context.Events.Remove(eventToDelete);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Notify clients via SignalR
            await _hubContext.Clients.All.SendAsync("LoadEvents");
            await _hubContext.Clients.All.SendAsync("LoadMyEvents");
			await _hubContext.Clients.All.SendAsync("UpdateTotalRegistered");

			// Redirect to a different page after deletion
			return RedirectToPage("/Users/Events/MyEvents"); // Adjust to the correct page
        }


    }
}
