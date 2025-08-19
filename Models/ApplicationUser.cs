using Microsoft.AspNetCore.Identity;
namespace Task_Manager.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName {  get; set; }=String.Empty;
    }
}
