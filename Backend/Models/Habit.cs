using System;
using System.Collections.Generic;

namespace HabitTracker.API.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public HabitFrequency Frequency { get; set; }
        public int TargetDays { get; set; }  // Number of days per frequency period to complete
        public string Color { get; set; }    // For UI customization
        public string Icon { get; set; }     // For UI customization
        public bool IsArchived { get; set; } = false;
        
        // Navigation properties
        public ICollection<HabitLog> HabitLogs { get; set; }
    }
    
    public enum HabitFrequency
    {
        Daily = 0,
        Weekly = 1,
        Monthly = 2
    }
} 