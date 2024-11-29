using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Pages.Users
{
    public class HomeModel : PageModel
    {
        private readonly Asm3EventManagementContext _context;
        private readonly IHubContext<SignalRHub> _hubContext;
		public HomeModel(Asm3EventManagementContext context, IHubContext<SignalRHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public IList<Event> Events { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            // Check if the session is null
           
            
            Events = await _context.Events.OrderByDescending(x =>x.EventId).Include(x => x.Category).Where(x => x.Status == 1).ToListAsync();

			

			return Page();
        }

        public IActionResult OnPostLogout()
		{
			HttpContext.Session.Clear();
			return RedirectToPage("/Users/Home");
		}
	}
   
}
