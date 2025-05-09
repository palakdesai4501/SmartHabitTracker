using System;
using System.Collections.Generic;
using HabitTracker.API.Models;

namespace HabitTracker.API.DTOs
{
    public class HabitDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public HabitFrequency Frequency { get; set; }
        public int TargetDays { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public bool IsArchived { get; set; }
        public int CompletedDays { get; set; }
        public double CompletionRate { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
    }
    
    public class HabitForCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public HabitFrequency Frequency { get; set; }
        public int TargetDays { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
    }
    
    public class HabitForUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public HabitFrequency Frequency { get; set; }
        public int TargetDays { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public bool IsArchived { get; set; }
    }
} 