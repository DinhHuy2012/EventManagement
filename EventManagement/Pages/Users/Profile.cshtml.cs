using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Pages.Users
{
	public class ProfileModel : PageModel
	{
		private readonly Asm3EventManagementContext _context;
		private readonly IHubContext<SignalRHub> _hubContext;

		public ProfileModel(Asm3EventManagementContext context, IHubContext<SignalRHub> hubContext)
		{
			_context = context;
			_hubContext = hubContext;
		}
		[BindProperty]
		public User Users { get; set; }
		[BindProperty]
		public string CurrentPassword { get; set; }

		[BindProperty]
		public string NewPassword { get; set; }

		[BindProperty]
		public string ConfirmPassword { get; set; }
		public async Task<IActionResult> OnGetAsync(int? Id)
		{
			// Check if the session is null
			if (HttpContext.Session.GetString("userID") == null)
			{
				// Redirect to the login page if the session is null
				return RedirectToPage("/Users/Login");
			}

			// Ensure the session value is captured correctly
			var userID = HttpContext.Session.GetString("userID");


			// Fetch the user by Id
			var user = await GetUserByIdAsync(int.Parse(userID)	);// Lấy thông tin user.

			if (Id == null || userID == null)
			{
				return RedirectToPage("/Users/Error404");
			}

			// Optionally, you could replace the following line with any logic
			ViewData["SuccessMessageUpdateProfile"] = TempData["SuccessMessageUpdateProfile"] as string;
			ViewData["SuccessMessageUpdatePassword"] = TempData["SuccessMessageUpdatePassword"] as string;

			Users = user;

			return Page();
		}


		public async Task<IActionResult> OnPostAsync(int? Id)
		{
			// Check if the Id is provided
			if (!Id.HasValue)
			{
				return NotFound();
			}
			if (HttpContext.Session.GetString("userID") == null)
			{
				// Redirect to the login page if the session is null
				return RedirectToPage("/Users/Login");
			}
			// Fetch the user to update
			var ToUpdate = await GetUserByIdAsync(Id.Value);

			// If no user is found, return NotFound
			if (ToUpdate == null)
			{
				return NotFound();
			}

			// Update properties
			ToUpdate.Fullname = Users.Fullname;
			ToUpdate.Phone = Users.Phone;

			// Save changes to the database
			await _context.SaveChangesAsync();

			// Notify all clients about the update
			await _hubContext.Clients.All.SendAsync("LoadUsers");

			// Set a success message in TempData
			TempData["SuccessMessageUpdateProfile"] = "Update successful!";

			// Redirect to the same page or a specific page to show the success message
			return RedirectToPage("/Users/Profile", new { Id = Id });
		}


		public IActionResult OnPostLogout()
		{
			HttpContext.Session.Clear();
			return RedirectToPage("/Users/Home");
		}


		public async Task<IActionResult> OnPostChangePasswordAsync()
		{
			var userID = HttpContext.Session.GetString("userID");
			var user = await GetUserByIdAsync(int.Parse(userID));
			ViewData["User"] = user;
			if (string.IsNullOrEmpty(userID))
			{
				return RedirectToPage("/Users/Login"); // Hoặc một trang khác để xử lý đăng nhập lại
			}


			// Validate current password
			if (string.IsNullOrEmpty(CurrentPassword))
			{
				ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không được để trống");
				return Page();

			}

			// Validate new password
			if (string.IsNullOrEmpty(NewPassword))
			{
				ModelState.AddModelError("NewPassword", "Mật khẩu mới không được để trống");
				return Page();

			}

			// Validate confirm password
			if (string.IsNullOrEmpty(ConfirmPassword))
			{
				ModelState.AddModelError("ConfirmPassword", "Nhập lại mật khẩu không được để trống");
				return Page();

			}

			// Verify the current password
			if (user.Password != CurrentPassword)
			{
				ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng !");
				return Page();

			}

			// Check if new passwords match
			if (NewPassword != ConfirmPassword)
			{
				ModelState.AddModelError("ConfirmPassword", "Mật khẩu mới và xác thực mật khẩu mới không khớp !");
				return Page();

			}

			// Lấy người dùng từ cơ sở dữ liệu


			user.Password = ConfirmPassword; // Thay thế bằng mã hóa mật khẩu nếu cần
			await _context.SaveChangesAsync();

			// Thông báo thành công
			TempData["SuccessMessageUpdatePassword"] = "Cập nhật mật khẩu thành công!";
			await _hubContext.Clients.All.SendAsync("LoadUsers");

			return RedirectToPage("/Users/Profile", new { Id = user.UserId });
		}




		public async Task<User> GetUserByIdAsync(int userId)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
		}
		
	}
}
