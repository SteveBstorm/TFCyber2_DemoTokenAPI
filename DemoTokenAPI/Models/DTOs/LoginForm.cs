using System.ComponentModel.DataAnnotations;

namespace DemoTokenAPI.Models.DTOs
{
    public class LoginForm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
