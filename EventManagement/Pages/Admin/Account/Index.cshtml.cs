using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EventManagement.Pages.Admin.Account
{
	public class IndexModel : PageModel
	{
		private readonly Asm3EventManagementContext _context;

		public IndexModel(Asm3EventManagementContext context)
		{
			_context = context;
		}
		public IList<User> Users { get; set; } = default!;


		public async Task<IActionResult> OnGetAsync()
		{

		
			Users = await _context.Users.ToListAsync();
			return Page();

		}
		[BindProperty(SupportsGet = true)]
		public string SearchName { get; set; } = string.Empty;

		public ContentResult OnGetGetUsersAsync(string search, int pageNumber = 1, int pageSize = 5)
		{
			var query = string.IsNullOrEmpty(search)
				? _context.Users
				: _context.Users
					.Where(u => u.Username.Contains(search)
							 || u.Email.Contains(search)
							 || u.Fullname.Contains(search)
							 || u.Role.Contains(search)
							 || u.Phone.Contains(search));

			var totalUsers = query.Count(); // Get total count of users matching the search criteria
			var users = query.OrderByDescending(u => u.UserId)  // Replace with your desired sorting column
				 .Skip((pageNumber - 1) * pageSize)
				 .Take(pageSize)
				 .ToList();


			var result = new
			{
				Users = users,
				TotalUsers = totalUsers
			};

			string jsonStr = JsonSerializer.Serialize(result);
			return Content(jsonStr);
		}


	}
}
