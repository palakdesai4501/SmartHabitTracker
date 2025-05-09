using System;

namespace HabitTracker.API.DTOs
{
    public class HabitLogDto
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string Notes { get; set; }
    }
    
    public class HabitLogForCreationDto
    {
        public int HabitId { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string Notes { get; set; }
    }
    
    public class HabitLogForUpdateDto
    {
        public bool IsCompleted { get; set; }
        public string Notes { get; set; }
    }
}