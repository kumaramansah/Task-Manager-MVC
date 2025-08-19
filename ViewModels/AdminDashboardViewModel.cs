using Task_Manager.Models;

namespace Task_Manager.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalTasks { get; set; }
        public int PendingTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int CompletedTasks { get; set; }
        public List<TaskItem> RecentTasks { get; set; } = new List<TaskItem>();

        public double CompletionRate => TotalTasks > 0 ? (double)CompletedTasks / TotalTasks * 100 : 0;
        public double ProgressRate => TotalTasks > 0 ? (double)(InProgressTasks + CompletedTasks) / TotalTasks * 100 : 0;
    }
}
