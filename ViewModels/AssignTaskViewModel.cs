using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Task_Manager.ViewModels
{
    public class AssignTaskViewModel
    {
        public string? UserId { get; set; } = string.Empty;

        [ValidateNever]  // ✅ ignore model binding + validation
        public IEnumerable<SelectListItem> UserList { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "Task title is required")]
        [StringLength(100, ErrorMessage = "Title must be less than 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Due date is required")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(1);
    }
}
