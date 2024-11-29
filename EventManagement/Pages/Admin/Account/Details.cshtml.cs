using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace EventManagement.Pages.Admin.Account
{
	public class DetailsModel : PageModel
	{
		private readonly Asm3EventManagementContext _context;

		public DetailsModel(Asm3EventManagementContext context)
		{
			_context = context;
		}

		public User User { get; set; } = default!;
		public List<Attendee> Attendees { get; set; } = new List<Attendee>();

		public async Task<IActionResult> OnGetAsync(int? Id)
		{
			if (Id == null)
			{
				return NotFound();
			}

			User = await _context.Users.FirstOrDefaultAsync(m => m.UserId == Id);
			Attendees = await _context.Attendees.Include(x=> x.Event).Where(x => x.UserId == Id).ToListAsync();

			if (Id == null || User == null)
			{
				return NotFound();
			}

			return Page();
		}
	}


}
