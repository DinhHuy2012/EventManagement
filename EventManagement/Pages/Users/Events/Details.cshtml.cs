using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventManagement.Pages.Users.Events
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
        public User Users { get; set; }
        public Event Event { get; set; } = default!;

        [BindProperty]
        public int TotalRegistered { get; set; }
        public List<Attendee> Attendees { get; set; } = new List<Attendee>();

        public async Task<IActionResult> OnGetAsync(int? EventId)
        {
            if (HttpContext.Session.GetString("userID") == null)
            {
                // Redirect to the login page if the session is null
                return RedirectToPage("/Users/Login");
            }
            var userID = HttpContext.Session.GetString("userID");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == int.Parse(userID));// Lấy thông tin user.

            if (userID == null)
            {
                return RedirectToPage("/Users/Error404");
            }


            Users = user;
            // If the Id is null or invalid, remain on the page
            if (!EventId.HasValue)
            {
                return Page();
            }

            // Query the event based on the Id
            Event = await _context.Events.Include(x => x.Category)
                                         .Include(x => x.Organizer)
                                         .FirstOrDefaultAsync(m => m.EventId == EventId);

            // If the event is not found, remain on the page
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
            Attendees = await _context.Attendees
                                      .OrderBy(x => x.RegistrationTime)
                                      .Include(x => x.Event)
                                      .Include(x => x.User)

                                      .Where(x => x.EventId == EventId)
                                      .ToListAsync();

            // Return the page
            return Page();
        }

        public async Task<IActionResult> OnGetApprove(int attendeesId)
        {
            var attendeesToUpdate = await _context.Attendees.FindAsync(attendeesId);
            if (attendeesToUpdate == null)
            {
                return NotFound();
            }
            attendeesToUpdate.Status = "Live"; // Ví dụ: 1 có thể là trạng thái "Approved"
		
			await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("LoadEvents");
            await _hubContext.Clients.All.SendAsync("LoadMyEvents");
			var totalRegistered = await _context.Attendees
												.Where(x => x.EventId == attendeesToUpdate.EventId && x.Status == "Live")
												.CountAsync();
			Console.WriteLine("Total Registered: " + totalRegistered); // Kiểm tra giá trị
			await _hubContext.Clients.All.SendAsync("UpdateTotalRegistered", totalRegistered);
			return RedirectToPage("/Users/Events/Details", new { attendeesToUpdate.EventId });
        }
        
        public async Task<IActionResult> OnGetReject(int attendeesId)
        {
            var attendeesToUpdate = await _context.Attendees.FindAsync(attendeesId);
            if (attendeesToUpdate == null)
            {
                return NotFound();
            }

            attendeesToUpdate.Status = "Reject"; // Ví dụ: 1 có thể là trạng thái "Approved"
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("LoadEvents");
            await _hubContext.Clients.All.SendAsync("LoadMyEvents");
			await _hubContext.Clients.All.SendAsync("UpdateTotalRegistered");
			var totalRegistered = await _context.Attendees
												.Where(x => x.EventId == attendeesToUpdate.EventId && x.Status == "Live")
												.CountAsync();
			Console.WriteLine("Total Registered: " + totalRegistered); // Kiểm tra giá trị
			await _hubContext.Clients.All.SendAsync("UpdateTotalRegistered", totalRegistered);
			return RedirectToPage("/Users/Events/Details", new { attendeesToUpdate.EventId });
        }



    }
}
