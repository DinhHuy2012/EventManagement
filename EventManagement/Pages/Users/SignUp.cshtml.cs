using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EventManagement.Pages.Users
{
    public class SignUpModel : PageModel
    {
		private readonly Asm3EventManagementContext _context;
		private readonly IHubContext<SignalRHub> _signalrHub;

		public SignUpModel(Asm3EventManagementContext context, IHubContext<SignalRHub> signalrHub)
		{
			_context = context;
			_signalrHub = signalrHub;
		}
	
		public void OnGet()
		{
			Page();
		}
		[BindProperty]
		public User User { get; set; }
		[BindProperty]
		public string ConfirmPassword { get; set; }

		public async Task<IActionResult> OnPost()
		{
		

			// Ensure that Email is not null or empty
			if (string.IsNullOrEmpty(User.Email))
			{
				ModelState.AddModelError("User.Email", "Email is required.");
				return Page();
			}
			var usernameExist = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Username);
			var emailExist = await _context.Users.FirstOrDefaultAsync(e => e.Email == User.Email);

			if (usernameExist != null)
			{
				ModelState.AddModelError("User.Username", "Username already exists");
				return Page();

			}

			if (emailExist != null)
			{
				ModelState.AddModelError("User.Email", "Email already exists");
				return Page();

			}

			if (User.Password != ConfirmPassword)
			{
				ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
				return Page();

			}
			User.Role = "Người đăng kí";
			User.Avatar = "avatar-trang.jpg";
			User.Status = true;
			User.JoinDate = DateTime.Now;
			_context.Users.Add(User);
			await _context.SaveChangesAsync();
			await _signalrHub.Clients.All.SendAsync("LoadUsers");
			TempData["SuccessMessage"] = "Registration successful! Please log in.";

			return RedirectToPage("/Users/Login");
		}

		public IActionResult OnPostLogout()
		{
			HttpContext.Session.Clear();
			return RedirectToPage("/Users/Home");
		}

	}
}
