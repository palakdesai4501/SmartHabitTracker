using System;

namespace HabitTracker.API.Models
{
    public class HabitLog
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public Habit Habit { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string Notes { get; set; }
    }
} 