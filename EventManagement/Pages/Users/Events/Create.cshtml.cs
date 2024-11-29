using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Pages.Users.Events
{
    public class CreateModel : PageModel
	{
		private readonly Asm3EventManagementContext _context;
		private readonly IHubContext<SignalRHub> _hubContext;

		public CreateModel(Asm3EventManagementContext context, IHubContext<SignalRHub> hubContext)
		{
			_context = context;
			_hubContext = hubContext;
		}
		public User Users { get; set; }

		[BindProperty]
		public Event Events { get; set; }

        public List<EventCategory> EventCategory { get; set; } = new List<EventCategory>();

        public async Task<IActionResult> OnGetAsync()
		{
			// Check if the session is null
			if (HttpContext.Session.GetString("userID") == null)
			{
				// Redirect to the login page if the session is null
				return RedirectToPage("/Users/Login");
			}
			var userID = HttpContext.Session.GetString("userID");
			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == int.Parse(userID));// Lấy thông tin user.
			EventCategory = await _context.EventCategories.ToListAsync();

            if ( userID == null)
			{
				return RedirectToPage("/Users/Error404");
			}


			Users = user;

			return Page();
		}

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("userID") == null)
            {
                // Redirect to the login page if the session is null
                return RedirectToPage("/Users/Login");
            }

            var userID = HttpContext.Session.GetString("userID");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == int.Parse(userID)); // Get user info
            EventCategory = await _context.EventCategories.ToListAsync();

            if (user == null)
            {
                return RedirectToPage("/Users/Error404");
            }

            Users = user;

            if (!ModelState.IsValid)
            {
                // Log the validation errors
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                    }
                }

                // Return to the page with validation errors
                return Page();
            }
            Events.OrganizerId = int.Parse(userID); // Using the user object for OrganizerId
            Events.Status = 2; // Assuming 2 means some specific status (e.g. pending)
            _context.Events.Add(Events);
            await _context.SaveChangesAsync();

            // Notify all clients about the new event
            await _hubContext.Clients.All.SendAsync("LoadMyEvents");
			await _hubContext.Clients.All.SendAsync("UpdateTotalRegistered");

			// Add success message to TempData for feedback on redirection
			TempData["SuccessMessage"] = "Event created successfully.!";

            return RedirectToPage("/Users/Events/Create");
        }

        public IActionResult OnPostLogout()
        {
     
            HttpContext.Session.Clear();
            return RedirectToPage("/Home");
        }
    }
}
