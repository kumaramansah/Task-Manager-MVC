using System;
using System.ComponentModel.DataAnnotations;

namespace Task_Manager.Models
{
    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }

    public class TaskItem
    {
        public int Id { get; set; }
        [Required, StringLength(100)] public string Title { get; set; } = string.Empty;
        [Required, StringLength(500, MinimumLength = 1)] public string Description { get; set; } = string.Empty;
        [Required, DataType(DataType.Date)]
        [CustomValidation(typeof(TaskItem), nameof(ValidateDueDate))]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(1); // Default to tomorrow

        public static ValidationResult ValidateDueDate(DateTime dueDate, ValidationContext context)
        {
            // Allow past dates for existing tasks (editing scenario)
            // Only validate for new tasks
            if (context.ObjectInstance is TaskItem task && task.Id == 0)
            {
                if (dueDate < DateTime.Today)
                {
                    return new ValidationResult("Due date cannot be in the past");
                }
            }
            return ValidationResult.Success;
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required] public string UserId { get; set; } = string.Empty;

        public virtual ApplicationUser? User { get; set; }
        
        // ✅ Replace bool with enum
        [Required]
        public TaskStatus Status { get; set; } = TaskStatus.Pending;

    }

}
