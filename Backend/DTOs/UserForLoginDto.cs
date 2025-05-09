using System.ComponentModel.DataAnnotations;

namespace HabitTracker.API.DTOs
{
    public class UserForLoginDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
} 