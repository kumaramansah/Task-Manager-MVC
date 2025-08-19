using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task_Manager.Models;
using Task_Manager.ViewModels;

namespace Task_Manager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // 📌 Admin Dashboard
        public async Task<IActionResult> Index()
        {
            var dashboardViewModel = new AdminDashboardViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalTasks = await _context.TaskItems.CountAsync(),
                PendingTasks = await _context.TaskItems.CountAsync(t => t.Status == Task_Manager.Models.TaskStatus.Pending),
                InProgressTasks = await _context.TaskItems.CountAsync(t => t.Status == Task_Manager.Models.TaskStatus.InProgress),
                CompletedTasks = await _context.TaskItems.CountAsync(t => t.Status == Task_Manager.Models.TaskStatus.Completed),
                RecentTasks = await _context.TaskItems
                    .Include(t => t.User)
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboardViewModel);
        }

        // 📌 Manage Users
        public IActionResult ManageUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // 📌 Edit User (GET)
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName
            };
            return View(model);
        }

        // 📌 Edit User (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.UserName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["success"] = "User updated successfully!";
                return RedirectToAction(nameof(ManageUsers));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // 📌 Delete User
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["success"] = "User deleted successfully!";
                return RedirectToAction(nameof(ManageUsers));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return RedirectToAction(nameof(ManageUsers));
        }
    }
}
