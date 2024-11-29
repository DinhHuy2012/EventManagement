using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace EventManagement.Pages.Admin.Account
{
    public class DeleteModel : PageModel
    {
		private readonly Asm3EventManagementContext _context;
		private readonly IHubContext<SignalRHub> _hubContext;

		public DeleteModel(Asm3EventManagementContext context, IHubContext<SignalRHub> hubContext)
		{
			_context = context;
			_hubContext = hubContext;
		}

		[BindProperty]
		public User User { get; set; } = default!;
		public async Task<IActionResult> OnGetAsync(int? Id)
		{
			if (Id == null)
			{
				return NotFound();
			}

			User = await _context.Users.FirstOrDefaultAsync(m => m.UserId == Id);

			if (User == null)
			{
				return NotFound();
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? Id)
		{
			if (Id == null)
			{
				return NotFound();
			}

			var user = await _context.Users.FindAsync(Id);
			if (user == null)
			{
				return NotFound();
			}

			// Cập nhật bảng Attend để đặt UserId = null
			var attendances = _context.Attendees.Where(a => a.UserId == Id);
			foreach (var attendance in attendances)
			{
				attendance.UserId = null;
			}
			await _context.SaveChangesAsync(); // Lưu các thay đổi ở bảng Attend

			// Xóa người dùng
			_context.Users.Remove(user);
			await _context.SaveChangesAsync(); // Lưu các thay đổi ở bảng Users

			// Gửi thông báo cho tất cả clients qua SignalR
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
