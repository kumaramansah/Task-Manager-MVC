using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_Manager.Models;
using Task_Manager.ViewModels;

namespace Task_Manager.Controllers
{
    [Authorize] // all actions require login
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TasksController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Tasks
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            if (User.IsInRole("Admin"))
            {
                // Admin sees all tasks
                var allTasks = await _context.TaskItems
                    .Include(t => t.User)
                    .OrderBy(t => t.DueDate)
                    .ToListAsync();
                return View(allTasks);
            }
            else
            {
                // Normal users only see their own tasks
                var userTasks = await _context.TaskItems
                    .Where(t => t.UserId == user.Id)
                    .OrderBy(t => t.DueDate)
                    .ToListAsync();
                return View(userTasks);
            }

        }
        [HttpGet("Tasks/Create")]
        public async Task<IActionResult> Create()
        {
            var model = new AssignTaskViewModel();
            // ✅ Only for Admin: prepare dropdown of users
            if (User.IsInRole("Admin"))
            {
                var Users = await _userManager.Users.ToListAsync();
                model.UserList = Users.Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.Email
                }).ToList();
            }
            return View(model);
        }

        [HttpPost("Tasks/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AssignTaskViewModel model)
        {
            // ✅ Custom validation only for Admin
            if (User.IsInRole("Admin") && string.IsNullOrEmpty(model.UserId))
            {
                ModelState.AddModelError("UserId", "Please select a user.");
            }
            if (ModelState.IsValid)
            {
                var task = new TaskItem
                {
                    Title = model.Title,
                    Description = model.Description,
                    DueDate = model.DueDate

                };

                if (User.IsInRole("Admin"))
                {
                    // ✅ Admin chooses the user
                    task.UserId = model.UserId;
                }
                else
                {
                    // ✅ Normal user gets assigned to themselves
                    var currentUser = await _userManager.GetUserAsync(User);
                    task.UserId = currentUser.Id;
                }

                _context.Add(task);
                await _context.SaveChangesAsync();
                TempData["success"] = "✅ Task assigned successfully!";
                return RedirectToAction(nameof(Index));
            }

            // reload dropdown for admin if validation fails
            if (User.IsInRole("Admin"))
            {
                var users = await _userManager.Users.ToListAsync();
                model.UserList = users.Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.Email
                }).ToList();
            }

            return View(model); // ✅ return same model
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignTask()
        {
            var users = _userManager.Users.ToList();
            var model = new AssignTaskViewModel
            {
                UserList = users.Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.Email
                }).ToList()
            };

            return View(model);
        }

        // POST: /Tasks/AssignTask
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTask(AssignTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // ✅ repopulate dropdown
                model.UserList = _userManager.Users
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.Email
                    }).ToList();

                return View(model);
            }

            var taskItem = new TaskItem
            {
                UserId = model.UserId,
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                CreatedAt = DateTime.Now,
                Status = Task_Manager.Models.TaskStatus.Pending
            };

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            TempData["success"] = "✅ Task assigned successfully!";
            return RedirectToAction(nameof(Index));
        }// GET: /Tasks/Edit/5
         // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return View("EditTask", task);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItem task)
        {
            try
            {
                if (id != task.Id)
                {
                    return BadRequest();
                }

                var existingTask = await _context.TaskItems.FindAsync(id);
                if (existingTask == null)
                {
                    return NotFound();
                }

                // 🔑 Check permissions
                if (!User.IsInRole("Admin") && existingTask.UserId != _userManager.GetUserId(User))
                {
                    return Forbid();
                }

                // 🔑 Update fields
                if (User.IsInRole("Admin"))
                {
                    existingTask.Title = task.Title;
                    existingTask.Description = task.Description;
                    existingTask.DueDate = task.DueDate;
                }

                existingTask.Status = task.Status;

                // Clear any validation errors for navigation properties when editing
                ModelState.Remove("DueDate");
                ModelState.Remove("User");
                
                // Validate the updated task
                if (!ModelState.IsValid)
                {
                    return View("EditTask", existingTask);
                }

                _context.Update(existingTask);
                await _context.SaveChangesAsync();

                TempData["success"] = "✅ Task updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TaskItems.Any(e => e.Id == task.Id))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while updating the task. Please try again.");
                return View("EditTask", task);
            }
        }


        // GET: /Tasks/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return NotFound();

            return View(task);
        }
        // GET: /Tasks/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return NotFound();

            // show confirmation view
            return View(task);
        }

        // POST: /Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return NotFound();

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: /Tasks/UpdateStatus/5
        [HttpGet]
        [Authorize]  // Any logged-in user
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return NotFound();

            // ✅ Prevent editing someone else’s task unless Admin
            if (!User.IsInRole("Admin") && task.UserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            return View(task);
        }
        // POST: /Tasks/UpdateStatus/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, TaskItem model)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return NotFound();

            if (!User.IsInRole("Admin") && task.UserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            // ✅ Update only status
            task.Status = model.Status;
            _context.Update(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
