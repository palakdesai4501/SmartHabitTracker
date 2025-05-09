using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HabitTracker.API.Data;
using HabitTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.API.Services
{
    public interface IHabitService
    {
        Task<IEnumerable<Habit>> GetHabitsForUser(int userId);
        Task<Habit> GetHabitById(int id, int userId);
        Task<Habit> CreateHabit(Habit habit);
        Task<Habit> UpdateHabit(Habit habit);
        Task<bool> DeleteHabit(int id, int userId);
        Task<int> GetCurrentStreak(int habitId);
        Task<int> GetLongestStreak(int habitId);
        Task<double> GetCompletionRate(int habitId);
    }
    
    public class HabitService : IHabitService
    {
        private readonly ApplicationDbContext _context;
        
        public HabitService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Habit>> GetHabitsForUser(int userId)
        {
            return await _context.Habits
                .Include(h => h.HabitLogs)
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.CreatedDate)
                .ToListAsync();
        }
        
        public async Task<Habit> GetHabitById(int id, int userId)
        {
            return await _context.Habits
                .Include(h => h.HabitLogs)
                .FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);
        }
        
        public async Task<Habit> CreateHabit(Habit habit)
        {
            await _context.Habits.AddAsync(habit);
            await _context.SaveChangesAsync();
            
            return habit;
        }
        
        public async Task<Habit> UpdateHabit(Habit habit)
        {
            _context.Habits.Update(habit);
            await _context.SaveChangesAsync();
            
            return habit;
        }
        
        public async Task<bool> DeleteHabit(int id, int userId)
        {
            var habit = await _context.Habits.FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);
            
            if (habit == null)
                return false;
                
            _context.Habits.Remove(habit);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<int> GetCurrentStreak(int habitId)
        {
            var habit = await _context.Habits
                .Include(h => h.HabitLogs)
                .FirstOrDefaultAsync(h => h.Id == habitId);
                
            if (habit == null)
                return 0;
                
            var logs = habit.HabitLogs
                .OrderByDescending(l => l.Date)
                .ToList();
                
            if (!logs.Any() || !logs.First().IsCompleted)
                return 0;
                
            int streak = 1;
            DateTime previousDate = logs.First().Date;
            
            for (int i = 1; i < logs.Count; i++)
            {
                var currentLog = logs[i];
                
                if (!currentLog.IsCompleted)
                    break;
                    
                TimeSpan difference = previousDate - currentLog.Date;
                
                if (difference.Days == 1)
                {
                    streak++;
                    previousDate = currentLog.Date;
                }
                else
                {
                    break;
                }
            }
            
            return streak;
        }
        
        public async Task<int> GetLongestStreak(int habitId)
        {
            var habit = await _context.Habits
                .Include(h => h.HabitLogs)
                .FirstOrDefaultAsync(h => h.Id == habitId);
                
            if (habit == null)
                return 0;
                
            var logs = habit.HabitLogs
                .Where(l => l.IsCompleted)
                .OrderBy(l => l.Date)
                .ToList();
                
            if (!logs.Any())
                return 0;
                
            int currentStreak = 1;
            int longestStreak = 1;
            DateTime previousDate = logs.First().Date;
            
            for (int i = 1; i < logs.Count; i++)
            {
                var currentLog = logs[i];
                TimeSpan difference = currentLog.Date - previousDate;
                
                if (difference.Days == 1)
                {
                    currentStreak++;
                    
                    if (currentStreak > longestStreak)
                        longestStreak = currentStreak;
                }
                else if (difference.Days > 1)
                {
                    currentStreak = 1;
                }
                
                previousDate = currentLog.Date;
            }
            
            return longestStreak;
        }
        
        public async Task<double> GetCompletionRate(int habitId)
        {
            var habit = await _context.Habits
                .Include(h => h.HabitLogs)
                .FirstOrDefaultAsync(h => h.Id == habitId);
                
            if (habit == null)
                return 0;
                
            int totalDays = (int)(DateTime.UtcNow - habit.CreatedDate).TotalDays + 1;
            int completedDays = habit.HabitLogs.Count(l => l.IsCompleted);
            
            return totalDays > 0 ? (double)completedDays / totalDays * 100 : 0;
        }
    }
}