using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Pages.Users.Events
{
    public class EditModel : PageModel
    {
        private readonly Asm3EventManagementContext _context;
        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(Asm3EventManagementContext context, IHubContext<SignalRHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public User Users { get; set; }
        public List<EventCategory> EventCategory { get; set; } = new List<EventCategory>();

        [BindProperty]
        public Event Events { get; set; }
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
            if (EventId == null)
            {
                return NotFound();
            }

            var events = await _context.Events.FirstOrDefaultAsync();
            EventCategory = await _context.EventCategories.ToListAsync();

            if (events == null)
            {
                return NotFound();
            }
            Events = events;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? EventId)
        {
            var ToUpdate = await _context.Events.FirstOrDefaultAsync(p => p.EventId == EventId);

            if (ToUpdate == null)
            {
                return NotFound();
            }

            // Update properties here
            ToUpdate.Title = Events.Title;
            ToUpdate.Description = Events.Description;
            ToUpdate.Location = Events.Location;
            ToUpdate.StartTime = Events.StartTime;
            ToUpdate.EndTime = Events.EndTime;
            ToUpdate.Capacity = Events.Capacity;
            ToUpdate.CategoryId = Events.CategoryId;
            ToUpdate.Image = Events.Image;

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("LoadMyEvents");
            await _hubContext.Clients.All.SendAsync("LoadEvents");

            TempData["SuccessMessageEdit"] = "Event edited successfully.!";

            return RedirectToPage("/Users/Events/Edit", new {ToUpdate.EventId });
        }
    }
}

