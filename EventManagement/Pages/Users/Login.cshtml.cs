using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.Pages.Users
{
    public class LoginModel : PageModel
    {
        private readonly Asm3EventManagementContext _context;

        public LoginModel(Asm3EventManagementContext context)
        {
            _context = context;
        }
        [BindProperty]
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public IActionResult OnGet()
        {

            return Page(); 
        }
        public IActionResult OnPost()
        {
            // Validate input
          

            // Check if user exists and password is correct
            var userExist = _context.Users.FirstOrDefault(e => e.Username == Username);
            if (userExist == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            if (userExist.Password != Password)
            {
                ModelState.AddModelError(string.Empty, "Invalid Username or password.");
                return Page();
            }

            // Set session variables
            HttpContext.Session.SetString("userName", userExist.Username);
            HttpContext.Session.SetString("userEmail", userExist.Email);
			HttpContext.Session.SetString("avatar", userExist.Avatar);
			HttpContext.Session.SetString("role", userExist.Role);
			
			HttpContext.Session.SetString("userID", userExist.UserId.ToString());

            // Redirect to the home page
            return RedirectToPage("/Users/Home");
        }
		public IActionResult OnPostLogout()
		{
			HttpContext.Session.Clear(); 
			return RedirectToPage("/Users/Home"); 
		}

	}
}
