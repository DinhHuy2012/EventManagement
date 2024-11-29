using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace EventManagement.Pages.Admin.Account
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
		public IActionResult OnGet()
		{
			if (!User.Identity.IsAuthenticated || HttpContext.Session.GetString("Role") != "Admin")
			{
				// Nếu không phải Admin hoặc chưa đăng nhập, chuyển hướng đến trang đăng nhập hoặc trang khác
				return RedirectToPage("/Users/Login");
			}

			return Page();
		}
		[BindProperty]
		public User Users { get; set; } = default!;

		public async Task<IActionResult> OnPostAsync()
		{

			if (!ModelState.IsValid)
			{
				// Return to the page with validation errors
				return Page();
			}

			// Check for uniqueness of Username
			if (await _context.Users.AnyAsync(u => u.Username == Users.Username))
			{
				ModelState.AddModelError("User.Username", "Username is already taken.");
				return Page();
			}

			// Check for uniqueness of Email
			if (await _context.Users.AnyAsync(u => u.Email == Users.Email))
			{
				ModelState.AddModelError("User.Email", "Email is already registered.");
				return Page();
			}
			Users.Avatar = "avatar-trang.jpg";
			Users.JoinDate = DateTime.Now;
			_context.Users.Add(Users);
			await _context.SaveChangesAsync();

			// Notify all clients about the new user
			await _hubContext.Clients.All.SendAsync("LoadUsers");

			return RedirectToPage("./Index");
		}
		public IActionResult OnPostLogout()
		{
			HttpContext.Session.Remove("user");
			HttpContext.Session.Remove("userID");
			HttpContext.Session.Remove("userEmail");
			HttpContext.Session.Clear();
			return RedirectToPage("/Index");
		}

	}
}
