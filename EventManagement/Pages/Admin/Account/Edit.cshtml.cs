using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace EventManagement.Pages.Admin.Account
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
		[BindProperty]
		public User Users { get; set; }

		public async Task<IActionResult> OnGetAsync(int? Id)
		{
			if (Id == null)
			{
				return NotFound();
			}

			var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == Id);
			if (user == null)
			{
				return NotFound();
			}
			Users = user;
			return Page();
		}
		public async Task<IActionResult> OnPostAsync(int? Id)
		{


			var ToUpdate = await _context.Users.FirstOrDefaultAsync(p => p.UserId == Id);

			if (ToUpdate == null)
			{
				return NotFound();
			}

			// Update properties here
			ToUpdate.Password = Users.Password;
			ToUpdate.Fullname = Users.Fullname;
			ToUpdate.Phone = Users.Phone;
			ToUpdate.Status = Users.Status;
			await _context.SaveChangesAsync();
			await _hubContext.Clients.All.SendAsync("LoadUsers");

			return RedirectToPage("./Index");
		}

	}
}
