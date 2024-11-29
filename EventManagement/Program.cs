using Assignment3_PhamDinhHuy;
using EventManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Services.AddDbContext<Asm3EventManagementContext>(option =>
{
	option.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
	options.Cookie.Name = "Asm3_EventManagement";
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Session tồn tại trong 30 phút
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStatusCodePages(async context =>
{
	var response = context.HttpContext.Response;
			if (response.StatusCode == 404)
			{
				response.Redirect("/Error404");
			}
});
app.MapGet("/", async context =>
{
	context.Response.Redirect("/Home");
});
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseSession();

app.UseEndpoints(endpoints =>
{
	endpoints.MapRazorPages();
	endpoints.MapHub<SignalRHub>("/SignalRHub");
});
app.Run();
